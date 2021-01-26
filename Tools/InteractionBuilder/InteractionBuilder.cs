using Godot;
using System;
using System.Collections.Generic;

using SpaceDoom.Tools.Dialog;
using SpaceDoom.Library.Abstract;

//Code for interaction writer GUI
public class InteractionBuilder : Control
{
    //UI Elements 
    private TextEdit LineEditor { get; set; }
    private LineEdit FrameEdit { get; set; }
    private LineEdit NameEdit { get; set; }
    private ItemList LineList { get; set; }
    private RichTextLabel LineDescription { get; set; }
    private Button SaveButton { get; set; }

    //Data properties
    private List<Line> lineColleciton { get; set; }
    private int CurrentListIndex { get; set; }

    public override void _Ready()
    {
        LineEditor = GetNode<TextEdit>("LineEditor");
        FrameEdit = GetNode<LineEdit>("FrameEdit");
        NameEdit = GetNode<LineEdit>("NameEdit");
        LineList = GetNode<ItemList>("LineList");
        LineDescription = GetNode<RichTextLabel>("LineDescription");

        SaveButton = GetNode<Button>("ButtonSave");
        SaveButton.Hide();

        lineColleciton = new List<Line>();
        CurrentListIndex = 0;
    }

    //Signals raised by the UI elements
    public void SaveButtonPress()
    {
        //The save button only appears when editing from the list. Saves text and replaces current index in list and GUI
        Line editedLine = new Line();
        if (!GrabLineFromGUI(out editedLine)) { return; }
        else { SaveData(editedLine, CurrentListIndex); }
        SaveButton.Hide();
        CurrentListIndex = lineColleciton.Count;
        ClearBoxes();
    }

    public void NewButtonPress()
    {
        //The new line button turns whatever data is in the boxes (if valid) into a new entry in the list and in the GUI
        Line newLine = new Line();
        if(!GrabLineFromGUI(out newLine)) { return; }
        else { SaveData(newLine); }
        CurrentListIndex = lineColleciton.Count;
        ClearBoxes();
    }

    public void DoneButtonPress()
    {
        //The done button takes the list of Lines and the overall name, and saves it into a json file
        if (string.IsNullOrEmpty(NameEdit.Text)) { return; }
        
        GD.Print("Done! Printing and writing to file!");
        foreach(var line in lineColleciton) { GD.Print($"{line.ImageIndex}:{line.Text}"); }
        try{
            InteractionConverter.BuildFile(lineColleciton, NameEdit.Text);
        }
        catch(Exception ex) {
            GD.Print("Writing to file failed! Message:");
            GD.Print(ex.Message);
        }

        GD.Print("Writing to file suceeded");
    }

    public void ItemSelected(int index)
    {
        SaveButton.Show();
        CurrentListIndex = index;
        LineEditor.Text = lineColleciton[CurrentListIndex].Text;
        FrameEdit.Text = lineColleciton[CurrentListIndex].ImageIndex.ToString();
    }

    //Methods to help funtionality of GUI
    private bool ContainsValidNumber(string text, out int frame)
    {
        if(int.TryParse(text, out frame))
        {
            if(frame < -1) { frame = -2; }
            return true;
        }

        return false;
    }

    //Save data with no index actually adds the data to the list, with index edits an entry
    private void SaveData(Line line)
    {
        lineColleciton.Add(line);
        RefreshList();
    }

    private void SaveData(Line line, int index)
    {
        lineColleciton[index] = line;
        RefreshList();
    }

    private bool GrabLineFromGUI(out Line line)
    {
        string text = LineEditor.Text;
        int frame;

        line = new Line();

        if (string.IsNullOrEmpty(text)) { return false; }
        if (!ContainsValidNumber(FrameEdit.Text, out frame))
        {
            //If the frame number is not valid
            frame = -2;
        }

        line.Text = text; line.ImageIndex = frame;
        return true;
    }

    private void RefreshList()
    {
        LineList.Clear();
        if(lineColleciton.Count == 0) { return; }
        foreach(var line in lineColleciton)
        {
            LineList.AddItem($"{line.ImageIndex}:{line.Text}");
        }
    }

    private void ClearBoxes()
    {
        //Clear out entry boxes
        LineEditor.Text = "";
        FrameEdit.Text = "";
    }
}