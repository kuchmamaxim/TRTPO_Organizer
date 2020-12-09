﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Organizer
{
    public partial class MainWindow : Form
    {
        private LoginWindow loginWindow;
        private NotesRedactor notesRedactor;
        private string userLogin;
        private SettingsWindow settingsWindow;
        private ShowNoteWindow noteWindow;
        private About about = new About();

        public MainWindow(LoginWindow login)
        {
            loginWindow = login;
            InitializeComponent();
            notesRedactor = new NotesRedactor();
            settingsWindow = new SettingsWindow(this) { Visible = false };
            noteWindow = new ShowNoteWindow(this) { Visible = false };
        }

        public void GetInformation(string _login)
        {
            userLogin = _login;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            this.searchNotesButton.Enabled = true;
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            noteGroupBox.Visible = true;
            AddNoteButton.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            
            int notificationNum = this.notificationComboBox.SelectedIndex;
            DateTime date = dateTimePicker.Value;
            string[] noteInfo = new string[] { this.noteName.Text, this.descriptionTextBox.Text, date.ToString() };

            if (notificationNum == -1)
            {
                Random random = new Random();
                errorLabel.ForeColor = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                errorLabel.Text = "Choose notification!";
            } else
            {
                errorLabel.Text = "";
                notesRedactor.CreateNote(userLogin, noteInfo, notificationNum);
                notesRedactor.CreateNote_History(userLogin, noteInfo, notificationNum);
                this.listOfNotes.Items.Add(noteInfo[0]);
                noteGroupBox.Visible = false;
                AddNoteButton.Enabled = true;
                noteName.Clear();
                descriptionTextBox.Clear();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            noteGroupBox.Visible = false;
            AddNoteButton.Enabled = true;
            noteName.Clear();
            descriptionTextBox.Clear();
        }

        private void listOfNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listOfNotes.SelectedIndex != -1)
            {
                this.deleteNoteButton.Enabled = true;
                this.showNoteButton.Enabled = true;
            }
        }

        private void deleteNoteButton_Click(object sender, EventArgs e)
        {
            notesRedactor.DeleteNote(userLogin, this.listOfNotes.SelectedItem.ToString());
            this.listOfNotes.Items.Clear();
            string st = notesRedactor.DownloadNotes(userLogin);
            string[] str = st.Split(',');
            this.listOfNotes.Items.AddRange(str);
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            settingsWindow.Show();
            settingsWindow.GetLogin(userLogin);
        }

        private void MainWindow_VisibleChanged(object sender, EventArgs e)
        {
            this.listOfNotes.Items.Clear();
            string st = notesRedactor.DownloadNotes(userLogin);
            if (st.Length > 0)
            {
                string[] str = st.Split(',');
                this.listOfNotes.Items.AddRange(str);
            }
            string user = notesRedactor.GetName(userLogin);
            if (user.Length > 0)
                this.nameLabel.Text = user;
            Image image = notesRedactor.GetFoto(userLogin);
            if (image != null)
                this.avatarBox.Image = image;
            this.showNoteButton.Enabled = false;
            this.deleteNoteButton.Enabled = false;
        }

        private void showNoteButton_Click(object sender, EventArgs e)
        {
            noteWindow.GetLogin(userLogin);
            noteWindow.SetName(this.listOfNotes.SelectedItem.ToString());
            this.Hide();
            noteWindow.Show();
        }

        private void searchNotesButton_Click(object sender, EventArgs e)
        {
            if (this.searchTextBox.TextLength > 0)
            {
                string st = notesRedactor.DownloadNotes(userLogin);
                string[] str = st.Split(',');

                string[] list = notesRedactor.Search(str, this.searchTextBox.Text);
                if (list[0] != "")
                {
                    this.listOfNotes.Items.Clear();
                    this.listOfNotes.Items.AddRange(list);
                }
            }
            else
            {
                this.listOfNotes.Items.Clear();
                string st = notesRedactor.DownloadNotes(userLogin);
                string[] str = st.Split(',');
                this.listOfNotes.Items.AddRange(str);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            loginWindow.Show();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            about.ShowDialog();
        }
    }
}
