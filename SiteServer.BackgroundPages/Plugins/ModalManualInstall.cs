﻿using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.Utils.Model.Enumerations;
using SiteServer.CMS.Plugin;

namespace SiteServer.BackgroundPages.Plugins
{
	public class ModalManualInstall : BasePage
	{
	    public RadioButtonList RblInstallType;

	    public PlaceHolder PhFile;
        public HtmlInputFile HifFile;

        public PlaceHolder PhIdAndVersion;
	    public TextBox TbPluginId;
	    public TextBox TbVersion;

        public static string GetOpenWindowString()
        {
            return LayerUtils.GetOpenScript("手动安装插件", PageUtils.GetPluginsUrl(nameof(ModalManualInstall), null), 560, 430);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            if (IsPostBack) return;

            EBooleanUtils.AddListItems(RblInstallType, "上传插件包", "指定插件Id与版本号");
            ControlUtils.SelectSingleItem(RblInstallType, true.ToString());
        }

        public void RblInstallType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhFile.Visible = TranslateUtils.ToBool(RblInstallType.SelectedValue);
            PhIdAndVersion.Visible = !PhFile.Visible;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(RblInstallType.SelectedValue))
            {
                if (HifFile.PostedFile == null || HifFile.PostedFile.FileName == "") return;

                var filePath = HifFile.PostedFile.FileName;
                if (!StringUtils.EqualsIgnoreCase(Path.GetExtension(filePath), ".nupkg"))
                {
                    FailMessage("必须上传后缀为.nupkg的文件");
                    return;
                }

                var idAndVersion = Path.GetFileNameWithoutExtension(filePath);
                var directoryPath = PathUtils.GetPackagesPath(idAndVersion);
                var localFilePath = PathUtils.Combine(directoryPath, idAndVersion + ".nupkg");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                HifFile.PostedFile.SaveAs(localFilePath);

                ZipUtils.UnpackFiles(localFilePath, directoryPath);

                string errorMessage;
                if (!PluginManager.Install(directoryPath, out errorMessage))
                {
                    FailMessage($"手动安装插件失败：{errorMessage}");
                    return;
                }

                Body.AddAdminLog("手动安装插件：" + idAndVersion);

                LayerUtils.CloseAndRedirect(Page, PageManagement.GetRedirectUrl());
            }
            else
            {
                string errorMessage;
                if (!PluginManager.GetAndInstall(TbPluginId.Text, TbVersion.Text, out errorMessage))
                {
                    FailMessage($"手动安装插件失败：{errorMessage}");
                    return;
                }

                Body.AddAdminLog($"手动安装插件：{TbPluginId.Text} {TbVersion.Text}");

                LayerUtils.CloseAndRedirect(Page, PageManagement.GetRedirectUrl());
            }
        }
	}
}
