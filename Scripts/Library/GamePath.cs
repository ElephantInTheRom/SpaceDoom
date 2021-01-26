using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceDoom.Library
{
    public static class GamePath
    {
        private static string CurrentDirectory = Directory.GetCurrentDirectory();
        public static string InteractionDirectory = CurrentDirectory + @"\Data\Interactions\";
        public static string ToolPath = CurrentDirectory + @"\Tools\";

        /// <summary>
        /// Returns the path to the spriteframes file given a name for a specified character, 
        /// this character name must match the name of their file in the interactions directory.
        /// </summary>
        public static string GetSpriteFramePath(string characterName)
        {
            return InteractionDirectory + characterName + @"\" + characterName + @".tres";
        }

        /// <summary>
        /// Returns the path to a specified dialog file given a character file name and interaction name
        /// this character name must match the name of their file in the interactions directory.
        /// </summary>
        public static string GetDialogFilePath(string characterName, string dialogName)
        {
            return InteractionDirectory + characterName + @"\Dialog\" + dialogName + @".json";
        }
    }
}