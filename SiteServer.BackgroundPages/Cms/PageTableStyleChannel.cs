﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.Utils.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.BackgroundPages.Cms
{
	public class PageTableStyleChannel : BasePageCms
    {
        public DropDownList DdlNodeId;
        public Repeater RptContents;
        public Button BtnAddStyle;
        public Button BtnAddStyles;
        public Button BtnImport;
        public Button BtnExport;

        private string _tableName;
        private NodeInfo _nodeInfo;
        private List<int> _relatedIdentities;
        private string _redirectUrl;

        public static string GetRedirectUrl(int publishmentSystemId, int nodeId)
        {
            return PageUtils.GetCmsUrl(nameof(PageTableStyleChannel), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"NodeID", nodeId.ToString()}
            });
        }

		public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            _tableName = DataProvider.NodeDao.TableName;
            var nodeId = Body.GetQueryInt("NodeID", PublishmentSystemId);
            _nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemId, nodeId);
            _redirectUrl = GetRedirectUrl(PublishmentSystemId, nodeId);
            _relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(PublishmentSystemId, nodeId);

            if (IsPostBack) return;

            VerifySitePermissions(AppManager.Permissions.WebSite.Configration);
                
            //删除样式
            if (Body.IsQueryExists("DeleteStyle"))
            {
                var attributeName = Body.GetQueryString("AttributeName");
                if (TableStyleManager.IsExists(_nodeInfo.NodeId, _tableName, attributeName))
                {
                    try
                    {
                        TableStyleManager.Delete(_nodeInfo.NodeId, _tableName, attributeName);
                        Body.AddSiteLog(PublishmentSystemId, "删除数据表单样式", $"表单:{_tableName},字段:{attributeName}");
                        SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        FailDeleteMessage(ex);
                    }
                }
            }

            NodeManager.AddListItems(DdlNodeId.Items, PublishmentSystemInfo, false, true, Body.AdminName);
            ControlUtils.SelectSingleItem(DdlNodeId, nodeId.ToString());

            RptContents.DataSource = TableStyleManager.GetTableStyleInfoList(_tableName, _relatedIdentities);
            RptContents.ItemDataBound += RptContents_ItemDataBound;
            RptContents.DataBind();

            BtnAddStyle.Attributes.Add("onclick", ModalTableStyleAdd.GetOpenWindowString(PublishmentSystemId, 0, _relatedIdentities, _tableName, string.Empty, _redirectUrl));
            BtnAddStyles.Attributes.Add("onclick", ModalTableStylesAdd.GetOpenWindowString(PublishmentSystemId, _relatedIdentities, _tableName, _redirectUrl));
            BtnImport.Attributes.Add("onclick", ModalTableStyleImport.GetOpenWindowString(_tableName, PublishmentSystemId, nodeId));
            BtnExport.Attributes.Add("onclick", ModalExportMessage.GetOpenWindowStringToSingleTableStyle(_tableName, PublishmentSystemId, nodeId));
        }

        public void Redirect(object sender, EventArgs e)
        {
            PageUtils.Redirect(GetRedirectUrl(PublishmentSystemId, TranslateUtils.ToInt(DdlNodeId.SelectedValue)));
        }

        private void RptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var styleInfo = (TableStyleInfo)e.Item.DataItem;

            var ltlAttributeName = (Literal)e.Item.FindControl("ltlAttributeName");
            var ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
            var ltlInputType = (Literal)e.Item.FindControl("ltlInputType");
            var ltlValidate = (Literal)e.Item.FindControl("ltlValidate");
            var ltlTaxis = (Literal)e.Item.FindControl("ltlTaxis");
            var ltlEditStyle = (Literal)e.Item.FindControl("ltlEditStyle");
            var ltlEditValidate = (Literal)e.Item.FindControl("ltlEditValidate");

            ltlAttributeName.Text = styleInfo.AttributeName;

            ltlDisplayName.Text = styleInfo.DisplayName;
            ltlInputType.Text = InputTypeUtils.GetText(styleInfo.InputType);

            ltlValidate.Text = ValidateTypeUtils.GetValidateInfo(styleInfo);

            var showPopWinString = ModalTableStyleAdd.GetOpenWindowString(PublishmentSystemId, styleInfo.TableStyleId, _relatedIdentities, _tableName, styleInfo.AttributeName, _redirectUrl);
            var editText = styleInfo.RelatedIdentity == _nodeInfo.NodeId ? "修改" : "添加";
            ltlEditStyle.Text = $@"<a href=""javascript:;"" onclick=""{showPopWinString}"">{editText}</a>";

            showPopWinString = ModalTableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleId, _relatedIdentities, _tableName, styleInfo.AttributeName, _redirectUrl);
            ltlEditValidate.Text = $@"<a href=""javascript:;"" onclick=""{showPopWinString}"">设置</a>";

            ltlTaxis.Text = styleInfo.Taxis.ToString();

            if (styleInfo.RelatedIdentity != _nodeInfo.NodeId) return;

            var urlStyle = PageUtils.GetCmsUrl(nameof(PageTableStyleChannel), new NameValueCollection
            {
                {"PublishmentSystemID", PublishmentSystemId.ToString()},
                {"NodeID", _nodeInfo.NodeId.ToString()},
                {"DeleteStyle", true.ToString()},
                {"TableName", _tableName},
                {"AttributeName", styleInfo.AttributeName}
            });
            ltlEditStyle.Text +=
                $@"&nbsp;&nbsp;<a href=""{urlStyle}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>";
        }
	}
}
