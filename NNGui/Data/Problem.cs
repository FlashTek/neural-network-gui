﻿using NNGui.Data.Links;
using NNGui.Data.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xml.Serialization;
using GongSolutions.Wpf.DragDrop;
using System.Windows.Controls;
using NNGui.Data.Optimizers;

namespace NNGui.Data
{
    public class Problem : IDeserializationCallback, INotifyPropertyChanged
    {
        public Problem()
        {
            NetworkArchitecture = new NetworkArchitecture(this);
            OutputDDHandler = new InternalOutputDDHandler(this);
            OptimizerSetting = new OptimizerSetting(OptimizerType.SGD);
        }

        public List<InputData> Inputs { get; } = new List<InputData>();
        //TODO: add support for this later
        //public List<OutputData> Outputs { get; set; }
        [XmlIgnore]
        public ObservableCollection<LinkBase> RawOutput { get; set; } = new ObservableCollection<LinkBase>();

        [XmlIgnore]
        public InternalOutputDDHandler OutputDDHandler { get; set; }

        public class InternalOutputDDHandler : GongSolutions.Wpf.DragDrop.IDropTarget
        {
            Problem _parent;

            public InternalOutputDDHandler(Problem parent)
            {
                _parent = parent;
            }

            public void DragOver(IDropInfo dropInfo)
            {
                var link = dropInfo.Data as LinkBase;
                if (link is null)
                    dropInfo.Effects = System.Windows.DragDropEffects.None;
                else
                    dropInfo.Effects = System.Windows.DragDropEffects.Copy;
            }

            public void Drop(IDropInfo dropInfo)
            {
                var link = dropInfo.Data as LinkBase;
                if (link is null)
                    return;

                _parent.Output.LinkConnection = new LinkConnection(link);
                ((ObservableCollection<LinkBase>)((ItemsControl)dropInfo.VisualTarget).ItemsSource).Clear();
                ((ObservableCollection<LinkBase>)((ItemsControl)dropInfo.VisualTarget).ItemsSource).Add(dropInfo.Data as LinkBase);
            }
        }

        public class OutputConfiguration : INotifyPropertyChanged
        {
            public OutputConfiguration() { }
            public OutputConfiguration(LinkConnection linkConnection)
            {
                LinkConnection = linkConnection;
            }

            private LinkConnection _linkConnection;
            public LinkConnection LinkConnection
            {
                get
                {
                    return _linkConnection;
                }
                set
                {
                    _linkConnection = value;
                    OnPropertyChanged("LinkConnection");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public OptimizerSetting OptimizerSetting { get; set; }


        private OutputConfiguration _output = new OutputConfiguration();
        public OutputConfiguration Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                if (!(RawOutput.Count == 1 && RawOutput[0] == value.LinkConnection.Target))
                {
                    RawOutput.Clear();
                    if (value.LinkConnection != null)
                        RawOutput.Add(value.LinkConnection.Target);
                }
                OnPropertyChanged("Output");
            }
        }
        public NetworkArchitecture NetworkArchitecture { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Export()
        {
            var serializer = new XmlCallbackSerializer(typeof(Problem), new Type[] {
                    typeof(NetworkArchitecture),
                    typeof(ActivationLayer), typeof(Convolution1DLayer), typeof(Convolution2DLayer), typeof(Convolution3DLayer), typeof(DenseLayer),
                    typeof(DropoutLayer), typeof(FlattenLayer), typeof(ReshapeLayer), typeof(MergeLayer),
                    typeof(ActivationFunctionParameter), typeof(DoubleParameter), typeof(IntParameter),
                    typeof(IntTuple2DParameter),  typeof(IntTuple3DParameter),  typeof(IntTuple4DParameter)
                });
            using (var sw = new System.IO.StreamWriter("network.xml"))
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, this);
                }
            }
        }
        public static Problem Import(List<InputData> inputs)
        {
            var serializer = new XmlCallbackSerializer(typeof(Problem), new Type[] {
                    typeof(NetworkArchitecture),
                    typeof(ActivationLayer), typeof(Convolution1DLayer), typeof(Convolution2DLayer), typeof(Convolution3DLayer), typeof(DenseLayer),
                    typeof(DropoutLayer), typeof(FlattenLayer), typeof(ReshapeLayer), typeof(MergeLayer),
                    typeof(ActivationFunctionParameter), typeof(DoubleParameter), typeof(IntParameter),
                    typeof(IntTuple2DParameter),  typeof(IntTuple3DParameter),  typeof(IntTuple4DParameter)
                });
            using (var sr = new System.IO.StreamReader("network.xml"))
            {
                using (XmlReader reader = XmlReader.Create(sr))
                {
                    Problem result = serializer.Deserialize(reader) as Problem;

                    foreach (var input in result.Inputs)
                    {
                        foreach (var oldInput in inputs)
                        {
                            if (input.ID.Equals(oldInput.ID))
                            {
                                input.Description = oldInput.Description;
                                inputs.Remove(oldInput);
                                break;
                            }
                        }
                    }

                    foreach (var item in inputs)
                        result.Inputs.Add(item);

                    result.NetworkArchitecture.ValidateInputCompatibility();

                    return result;
                }
            }
        }

        public void OnDeserialization(object sender)
        {
            NetworkArchitecture.Problem = this;

            //fix the Output node again
            if (Output != null && Output.LinkConnection != null)
            {
                foreach (var chain in NetworkArchitecture.Chains)
                {
                    foreach (var link in chain.ChainLinks)
                    {
                        if (link.ID.Equals(Output.LinkConnection.TargetID))
                        {
                            Output.LinkConnection.Target = link;
                            RawOutput.Clear();
                            RawOutput.Add(link);
                            break;
                        }
                    }
                }
            }
        }
    }
}
