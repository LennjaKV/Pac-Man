namespace PacMan.GUI
{
    partial class PacManGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // PacManGUI
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(700, 338);
            Margin = new Padding(3, 2, 3, 2);
            Name = "PacManGUI";
            Text = "PacManGUI";
            Load += PacManGUI_Load;
            KeyDown += PacManGUI_KeyDown;
            ResumeLayout(false);
        }

        #endregion
    }
}