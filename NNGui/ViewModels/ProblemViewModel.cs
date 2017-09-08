﻿using Caliburn.Micro;
using NNGui.Data;
using NNGui.ViewModels.Links;
using NNGui.ViewModels.Optimizers;
using NNGui.ViewModels.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNGui.ViewModels
{
    class ProblemViewModel : PropertyChangedBase
    {
        public Problem ProblemData { get; }

        public ProblemViewModel(Problem problem)
        {
            ProblemData = problem;
            NetworkArchitecture = new NetworkArchitectureViewModel(problem.NetworkArchitecture);
            OptimizerSetting = new OptimizerSettingViewModel(problem.OptimizerSetting);

            Inputs = new ObservableCollection<InputDataViewModel>();
            foreach (var input in problem.Inputs)
                Inputs.Add(new InputDataViewModel(input));

            OutputConfiguration = new OutputConfigurationViewModel(ProblemData.Output);
        }

        public NetworkArchitectureViewModel NetworkArchitecture { get; set; }
        public ObservableCollection<InputDataViewModel> Inputs { get; set; }

        public OptimizerSettingViewModel OptimizerSetting { get; set; }

        public OutputConfigurationViewModel OutputConfiguration { get; set; }
    }
}
