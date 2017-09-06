﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NNGui.Data.Parameters;

namespace NNGui.Data.Links
{
    [Serializable]
    public class MergeLayer : LinkBase
    {
        private MergeLayer() : base() { }
        public MergeLayer(Chain parent) : base(parent) { }

        public MergeLayer(Chain parent, string name) : base(parent, name)
        {
            Parameters.Add(new LinkConnectionListParameter("Links", this));
        }

        public override string TypeName { get { return "Merge Layer"; } }

        public override void ValidateInputCompatibility()
        {
            IsInputCompatible = true;

            //now check, of we have to make this false again
            var list = (Parameters[0] as LinkConnectionListParameter).Value;
            if (list.Count > 0)
            {
                //check the ranks
                int? rawRank = list[0].Target.GetTensorRank();
                if (!rawRank.HasValue)
                {
                    IsInputCompatible = false;
                    return;
                }
                int rank = rawRank.Value;
                for (int i = 1; i < list.Count; i++)
                {
                    if (!rawRank.Equals(list[i].Target.GetTensorRank()))
                    {
                        IsInputCompatible = false;
                        return;
                    }
                }
            }
        }

        public override int? GetTensorRank()
        {
            if ((Parameters[0] as LinkConnectionListParameter).Value.Count > 0)
            {
                return (Parameters[0] as LinkConnectionListParameter).Value[0].Target.GetTensorRank();
            }
            return null;
        }
    }
}
