namespace Launcher
{
    partial class CreateAccountForm
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
            label1 = new System.Windows.Forms.Label();
            nameTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            passwordTextBox = new System.Windows.Forms.TextBox();
            verifyTextBox = new System.Windows.Forms.TextBox();
            createButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 13);
            label1.TabIndex = 0;
            label1.Text = "Player Name:";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new System.Drawing.Point(128, 13);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new System.Drawing.Size(504, 20);
            nameTextBox.TabIndex = 1;
            nameTextBox.TextChanged += new System.EventHandler(nameTextBox_TextChanged);
            nameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(nameTextBox_KeyPress);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(16, 63);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 13);
            label2.TabIndex = 2;
            label2.Text = "Password:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(19, 113);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(85, 13);
            label3.TabIndex = 3;
            label3.Text = "Verify Password:";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new System.Drawing.Point(128, 63);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new System.Drawing.Size(504, 20);
            passwordTextBox.TabIndex = 4;
            // 
            // verifyTextBox
            // 
            verifyTextBox.Location = new System.Drawing.Point(128, 113);
            verifyTextBox.Name = "verifyTextBox";
            verifyTextBox.Size = new System.Drawing.Size(504, 20);
            verifyTextBox.TabIndex = 5;
            // 
            // createButton
            // 
            createButton.Location = new System.Drawing.Point(22, 225);
            createButton.Name = "createButton";
            createButton.Size = new System.Drawing.Size(123, 23);
            createButton.TabIndex = 6;
            createButton.Text = "Create Account";
            createButton.UseVisualStyleBackColor = true;
            createButton.Click += new System.EventHandler(createButton_Click);
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(557, 225);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += new System.EventHandler(cancelButton_Click);
            // 
            // CreateAccountForm
            // 
            AcceptButton = createButton;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(679, 295);
            Controls.Add(cancelButton);
            Controls.Add(createButton);
            Controls.Add(verifyTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(nameTextBox);
            Controls.Add(label1);
            Name = "CreateAccountForm";
            Text = "CreateAccountForm";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox verifyTextBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
    }
}