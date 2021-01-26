using System;

namespace SpaceDoom.Library.Abstract
{
    //This struct represents a single line of dialog accompaning a picture for the character. 
    //A negative one (-1) as the index represents a random image, and a -2 or out of bounds number will show nothing
    public struct Line
    {
        public string Text { get; set; }
        public int ImageIndex { get; set; }
    }
}