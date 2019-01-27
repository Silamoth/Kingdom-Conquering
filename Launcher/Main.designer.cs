namespace Launcher
{
    partial class Main
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
            label2 = new System.Windows.Forms.Label();
            kingdomNameTextBox = new System.Windows.Forms.TextBox();
            passwordTextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            loginButton = new System.Windows.Forms.Button();
            createAccountButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 121);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 13);
            label1.TabIndex = 0;
            label1.Text = "Player Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 201);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 13);
            label2.TabIndex = 1;
            label2.Text = "Password:";
            // 
            // kingdomNameTextBox
            // 
            kingdomNameTextBox.Location = new System.Drawing.Point(105, 121);
            kingdomNameTextBox.Name = "kingdomNameTextBox";
            kingdomNameTextBox.Size = new System.Drawing.Size(274, 20);
            kingdomNameTextBox.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new System.Drawing.Point(105, 201);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new System.Drawing.Size(274, 20);
            passwordTextBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.Location = new System.Drawing.Point(10, 62);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(77, 25);
            label3.TabIndex = 4;
            label3.Text = "Login:";
            // 
            // loginButton
            // 
            loginButton.Location = new System.Drawing.Point(15, 262);
            loginButton.Name = "loginButton";
            loginButton.Size = new System.Drawing.Size(75, 23);
            loginButton.TabIndex = 5;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += new System.EventHandler(loginButton_Click);
            // 
            // createAccountButton
            // 
            createAccountButton.Location = new System.Drawing.Point(15, 352);
            createAccountButton.Name = "createAccountButton";
            createAccountButton.Size = new System.Drawing.Size(138, 23);
            createAccountButton.TabIndex = 6;
            createAccountButton.Text = "Create An Account";
            createAccountButton.UseVisualStyleBackColor = true;
            createAccountButton.Click += new System.EventHandler(createAccountButton_Click);
            // 
            // Main
            // 
            AcceptButton = loginButton;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(707, 410);
            Controls.Add(createAccountButton);
            Controls.Add(loginButton);
            Controls.Add(label3);
            Controls.Add(passwordTextBox);
            Controls.Add(kingdomNameTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Main";
            Text = "Kingdom Conquering Launcher";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox kingdomNameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button createAccountButton;
    }
}

