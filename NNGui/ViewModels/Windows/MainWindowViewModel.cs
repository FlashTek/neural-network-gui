﻿using Caliburn.Micro;
using NNGui.Data;
using NNGui.Data.Parameters;
using NNGui.Data.Tuple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNGui.ViewModels.Windows
{
    class MainWindowViewModel : PropertyChangedBase
    {
        public MainWindowViewModel()
        {
            var problem = new Problem();
            foreach (var item in GetSampleInputData())
                problem.Inputs.Add(item);

            Problem = new ProblemViewModel(problem);
        }

        public static List<InputData> GetSampleInputData()
        {
            var result = new List<InputData>();
            result.Add(new InputData("State-Images", "State Images", new IntTuple3D(84, 84, 4), "The state conisting out of the last 4 images."));
            result.Add(new InputData("State-RAM", "State RAM", new IntTuple1D(1024 * 1024), "The state conisting out of the current RAM."));
            return result;
        }

        private ProblemViewModel _problem;
        public ProblemViewModel Problem
        {
            get
            {
                return _problem;
            }
            set
            {
                _problem = value;
                NotifyOfPropertyChange(() => Problem);
            }
        }

        public void Export()
        {
            Problem.ProblemData.Export();
        }

        public void Import()
        {
            Problem = new ProblemViewModel(Data.Problem.Import(GetSampleInputData()));
        }
    }
}
