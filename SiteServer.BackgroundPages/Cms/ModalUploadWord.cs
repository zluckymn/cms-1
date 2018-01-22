﻿using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.Utils.Model;
using SiteServer.BackgroundPages.Ajax;
using SiteServer.BackgroundPages.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Create;
using SiteServer.CMS.Core.Office;
using SiteServer.CMS.Model;

namespace SiteServer.BackgroundPages.Cms
{
    public class ModalUploadWord : BasePageCms
    {
        public CheckBox CbIsFirstLineTitle;
        public CheckBox CbIsFirstLineRemove;
        public CheckBox CbIsClearFormat;
        public CheckBox CbIsFirstLineIndent;
        public CheckBox CbIsClearFontSize;
        public CheckBox CbIsClearFontFamily;
        public CheckBox CbIsClearImages;
        public DropDownList DdlContentLevel;

        private NodeInfo _nodeInfo;
        private string _returnUrl;

        public static string GetOpenWindowString(int publishmentSystemId, int nodeId, string returnUrl)
        {
            return LayerUtils.GetOpenScript("批量导入Word文件",
                PageUtils.GetCmsUrl(nameof(ModalUploadWord), new NameValueCollection
                {
                    {"publishmentSystemID", publishmentSystemId.ToString()},
                    {"nodeID", nodeId.ToString()},
                    {"returnUrl", returnUrl}
                }), 600, 400);
        }

        public string GetUploadWordMultipleUrl()
        {
            return AjaxUploadService.GetUploadWordMultipleUrl(PublishmentSystemId);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            var nodeId = int.Parse(Body.GetQueryString("NodeID"));
            _nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemId, nodeId);
            _returnUrl = Body.GetQueryString("ReturnUrl");

            if (IsPostBack) return;

            int checkedLevel;
            var isChecked = CheckManager.GetUserCheckLevel(Body.AdminName, PublishmentSystemInfo, PublishmentSystemId, out checkedLevel);
            CheckManager.LoadContentLevelToEdit(DdlContentLevel, PublishmentSystemInfo, _nodeInfo.NodeId, null, isChecked, checkedLevel);
            ControlUtils.SelectSingleItem(DdlContentLevel, CheckManager.LevelInt.CaoGao.ToString());
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            var fileCount = TranslateUtils.ToInt(Request.Form["File_Count"]);
            if (fileCount == 1)
            {
                var fileName = Request.Form["fileName_1"];
                var redirectUrl = WebUtils.GetContentAddUploadWordUrl(PublishmentSystemId, _nodeInfo, CbIsFirstLineTitle.Checked, CbIsFirstLineRemove.Checked, CbIsClearFormat.Checked, CbIsFirstLineIndent.Checked, CbIsClearFontSize.Checked, CbIsClearFontFamily.Checked, CbIsClearImages.Checked, TranslateUtils.ToIntWithNagetive(DdlContentLevel.SelectedValue), fileName, _returnUrl);
                LayerUtils.CloseAndRedirect(Page, redirectUrl);

                return;
            }
            if (fileCount > 1)
            {
                var tableName = NodeManager.GetTableName(PublishmentSystemInfo, _nodeInfo);
                var relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(PublishmentSystemId, _nodeInfo.NodeId);
                var styleInfoList = TableStyleManager.GetTableStyleInfoList(tableName, relatedIdentities);

                for (var index = 1; index <= fileCount; index++)
                {
                    var fileName = Request.Form["fileName_" + index];
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var formCollection = WordUtils.GetWordNameValueCollection(PublishmentSystemId, CbIsFirstLineTitle.Checked, CbIsFirstLineRemove.Checked, CbIsClearFormat.Checked, CbIsFirstLineIndent.Checked, CbIsClearFontSize.Checked, CbIsClearFontFamily.Checked, CbIsClearImages.Checked, TranslateUtils.ToInt(DdlContentLevel.SelectedValue), fileName);

                        if (!string.IsNullOrEmpty(formCollection[ContentAttribute.Title]))
                        {
                            var contentInfo = new ContentInfo();

                            BackgroundInputTypeParser.SaveAttributes(contentInfo, PublishmentSystemInfo, styleInfoList, formCollection, ContentAttribute.AllAttributesLowercase);

                            contentInfo.NodeId = _nodeInfo.NodeId;
                            contentInfo.PublishmentSystemId = PublishmentSystemId;
                            contentInfo.AddUserName = Body.AdminName;
                            contentInfo.AddDate = DateTime.Now;
                            contentInfo.LastEditUserName = contentInfo.AddUserName;
                            contentInfo.LastEditDate = contentInfo.AddDate;

                            contentInfo.CheckedLevel = TranslateUtils.ToIntWithNagetive(DdlContentLevel.SelectedValue);
                            contentInfo.IsChecked = contentInfo.CheckedLevel >= PublishmentSystemInfo.CheckContentLevel;

                            contentInfo.Id = DataProvider.ContentDao.Insert(tableName, PublishmentSystemInfo, contentInfo);

                            if (contentInfo.IsChecked)
                            {
                                CreateManager.CreateContentAndTrigger(PublishmentSystemId, _nodeInfo.NodeId, contentInfo.Id);
                            }
                        }
                    }
                }
            }

            LayerUtils.Close(Page);
        }
    }
}
