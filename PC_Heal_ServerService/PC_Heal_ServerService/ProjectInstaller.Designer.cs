
namespace PC_Heal_ServerService
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
            this.PC_Heal_Server_Installer = new System.ServiceProcess.ServiceProcessInstaller();
            this.PC_Heal_ServerService = new System.ServiceProcess.ServiceInstaller();
            // 
            // PC_Heal_Server_Installer
            // 
            this.PC_Heal_Server_Installer.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PC_Heal_Server_Installer.Password = null;
            this.PC_Heal_Server_Installer.Username = null;
            // 
            // PC_Heal_ServerService
            // 
            this.PC_Heal_ServerService.Description = "PC Heal Server\'s Service";
            this.PC_Heal_ServerService.DisplayName = "PC_Heal_Server";
            this.PC_Heal_ServerService.ServiceName = "PC_Heal_Server";
            this.PC_Heal_ServerService.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PC_Heal_Server_Installer,
            this.PC_Heal_ServerService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PC_Heal_Server_Installer;
        private System.ServiceProcess.ServiceInstaller PC_Heal_ServerService;
    }
}