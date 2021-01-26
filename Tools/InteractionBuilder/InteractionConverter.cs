using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using SpaceDoom.Library.Abstract;

/*
 Here is how dialog is constructed and processed:
 - Building tool is used to make a collection of lines (text paired with a frame index)
 - Once submit is hit, data is sent to this class
 - This class will then take collection of lines and an interaction name and write it into the data folder
 - The Name of the file needs to be the same as the animation group!
 */

namespace SpaceDoom.Tools.Dialog
{ 
    public static class InteractionConverter
    {
        //If this ends up getting exported, change this file path to a local one
        private static string DataPath = Library.GamePath.ToolPath + @"InteractionBuilder\Data\";

        public static void BuildFile(List<Line> lines, string name)
        {
            Interaction interaction = new Interaction(name, lines);
            try
            {
                string serializedData = JsonConvert.SerializeObject(interaction);
                File.WriteAllText(DataPath + name + ".json", serializedData);
            }
            catch(Exception ex)
            {
                Godot.GD.Print("Error in writing data to path: ");
                Godot.GD.Print(ex.Message);
            }
        }

        public static Interaction BuildInteraction(string filePath)
        {
            string contents = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Interaction>(contents);
        }
    }
}