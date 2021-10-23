
namespace PC_Heal_ClientService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PC_Heal_Installer = new System.ServiceProcess.ServiceProcessInstaller();
            this.PC_Heal = new System.ServiceProcess.ServiceInstaller();
            // 
            // PC_Heal_Installer
            // 
            this.PC_Heal_Installer.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PC_Heal_Installer.Password = null;
            this.PC_Heal_Installer.Username = null;
            // 
            // PC_Heal
            // 
            this.PC_Heal.Description = "PC_Heal\'s Service";
            this.PC_Heal.DisplayName = "PC_Heal";
            this.PC_Heal.ServiceName = "PC_Heal";
            this.PC_Heal.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PC_Heal_Installer,
            this.PC_Heal});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PC_Heal_Installer;
        private System.ServiceProcess.ServiceInstaller PC_Heal;
    }
}