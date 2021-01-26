using System;
using System.Collections.Generic;

namespace SpaceDoom.Library.Abstract
{
    public class Interaction
    {
        public string AnimationGroup { get; set; }
        public List<Line> Lines { get; set; }

        public Interaction(string animationGroup, List<Line> lines)
        {
            AnimationGroup = animationGroup;
            Lines = lines;
        }
    }
}