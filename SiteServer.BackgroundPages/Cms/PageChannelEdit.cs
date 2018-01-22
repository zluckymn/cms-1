﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.Utils.Model;
using SiteServer.Utils.Model.Enumerations;
using SiteServer.BackgroundPages.Controls;
using SiteServer.BackgroundPages.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Create;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.Enumerations;
using SiteServer.CMS.Plugin;

namespace SiteServer.BackgroundPages.Cms
{
    public class PageChannelEdit : BasePageCms
    {
        public TextBox TbNodeName;
        public TextBox TbNodeIndexName;
        public DropDownList DdlContentModelPluginId;
        public PlaceHolder PhContentRelatedPluginIds;
        public CheckBoxList CblContentRelatedPluginIds;
        public CheckBoxList CblNodeGroupNameCollection;
        public RadioButtonList RblIsChannelAddable;
        public RadioButtonList RblIsContentAddable;
        public TextBox TbLinkUrl;
        public DropDownList DdlLinkType;
        public DropDownList DdlTaxisType;
        public DropDownList DdlChannelTemplateId;
        public DropDownList DdlContentTemplateId;
        public TextBox TbImageUrl;
        public TextBox TbFilePath;
        public TextBox TbChannelFilePathRule;
        public TextBox TbContentFilePathRule;
        public TextEditorControl TbContent;
        public TextBox TbKeywords;
        public TextBox TbDescription;
        public ChannelAuxiliaryControl CacAttributes;
        public Button BtnCreateChannelRule;
        public Button BtnCreateContentRule;
        public Button BtnSelectImage;
        public Button BtnUploadImage;
        public Button BtnSubmit;

        private int _nodeId;

        public static string GetRedirectUrl(int publishmentSystemId, int nodeId, string returnUrl)
        {
            return PageUtils.GetCmsUrl(nameof(PageChannelEdit), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"NodeID", nodeId.ToString()},
                {"ReturnUrl", StringUtils.ValueToUrl(returnUrl)}
            });
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl");

            _nodeId = Body.GetQueryInt("NodeID");
            ReturnUrl = StringUtils.ValueFromUrl(Body.GetQueryString("ReturnUrl"));

            if (Body.GetQueryString("CanNotEdit") == null && Body.GetQueryString("UncheckedChannel") == null)
            {
                if (!HasChannelPermissions(_nodeId, AppManager.Permissions.Channel.ChannelEdit))
                {
                    PageUtils.RedirectToErrorPage("您没有修改栏目的权限！");
                    return;
                }
            }
            if (Body.IsQueryExists("CanNotEdit"))
            {
                BtnSubmit.Visible = false;
            }

            var nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemId, _nodeId);
            if (nodeInfo == null) return;

            CacAttributes.PublishmentSystemInfo = PublishmentSystemInfo;
            CacAttributes.NodeId = _nodeId;

            if (!IsPostBack)
            {
                DdlContentModelPluginId.Items.Add(new ListItem("<默认>", string.Empty));
                var contentTables = PluginManager.GetContentModelPlugins();
                foreach (var contentTable in contentTables)
                {
                    DdlContentModelPluginId.Items.Add(new ListItem(contentTable.Title, contentTable.Id));
                }
                ControlUtils.SelectSingleItem(DdlContentModelPluginId, nodeInfo.ContentModelPluginId);

                var plugins = PluginManager.GetAllContentRelatedPlugins(false);
                if (plugins.Count > 0)
                {
                    var relatedPluginIds =
                        TranslateUtils.StringCollectionToStringList(nodeInfo.ContentRelatedPluginIds);
                    foreach (var pluginMetadata in plugins)
                    {
                        CblContentRelatedPluginIds.Items.Add(new ListItem(pluginMetadata.Title, pluginMetadata.Id)
                        {
                            Selected = relatedPluginIds.Contains(pluginMetadata.Id)
                        });
                    }
                }
                else
                {
                    PhContentRelatedPluginIds.Visible = false;
                }

                CacAttributes.Attributes = nodeInfo.Additional;

                TbImageUrl.Attributes.Add("onchange", GetShowImageScript("preview_NavigationPicPath", PublishmentSystemInfo.Additional.WebUrl));

                var showPopWinString = ModalFilePathRule.GetOpenWindowString(PublishmentSystemId, _nodeId, true, TbChannelFilePathRule.ClientID);
                BtnCreateChannelRule.Attributes.Add("onclick", showPopWinString);

                showPopWinString = ModalFilePathRule.GetOpenWindowString(PublishmentSystemId, _nodeId, false, TbContentFilePathRule.ClientID);
                BtnCreateContentRule.Attributes.Add("onclick", showPopWinString);

                showPopWinString = ModalSelectImage.GetOpenWindowString(PublishmentSystemInfo, TbImageUrl.ClientID);
                BtnSelectImage.Attributes.Add("onclick", showPopWinString);

                showPopWinString = ModalUploadImage.GetOpenWindowString(PublishmentSystemId, TbImageUrl.ClientID);
                BtnUploadImage.Attributes.Add("onclick", showPopWinString);

                ELinkTypeUtils.AddListItems(DdlLinkType);
                ETaxisTypeUtils.AddListItemsForChannelEdit(DdlTaxisType);

                CblNodeGroupNameCollection.DataSource = DataProvider.NodeGroupDao.GetDataSource(PublishmentSystemId);

                DdlChannelTemplateId.DataSource = DataProvider.TemplateDao.GetDataSourceByType(PublishmentSystemId, ETemplateType.ChannelTemplate);

                DdlContentTemplateId.DataSource = DataProvider.TemplateDao.GetDataSourceByType(PublishmentSystemId, ETemplateType.ContentTemplate);

                DataBind();

                DdlChannelTemplateId.Items.Insert(0, new ListItem("<未设置>", "0"));
                ControlUtils.SelectSingleItem(DdlChannelTemplateId, nodeInfo.ChannelTemplateId.ToString());

                DdlContentTemplateId.Items.Insert(0, new ListItem("<未设置>", "0"));
                ControlUtils.SelectSingleItem(DdlContentTemplateId, nodeInfo.ContentTemplateId.ToString());

                TbNodeName.Text = nodeInfo.NodeName;
                TbNodeIndexName.Text = nodeInfo.NodeIndexName;
                TbLinkUrl.Text = nodeInfo.LinkUrl;

                foreach (ListItem item in CblNodeGroupNameCollection.Items)
                {
                    item.Selected = CompareUtils.Contains(nodeInfo.NodeGroupNameCollection, item.Value);
                }
                TbFilePath.Text = nodeInfo.FilePath;
                TbChannelFilePathRule.Text = nodeInfo.ChannelFilePathRule;
                TbContentFilePathRule.Text = nodeInfo.ContentFilePathRule;

                ControlUtils.SelectSingleItem(DdlLinkType, nodeInfo.LinkType);
                ControlUtils.SelectSingleItem(DdlTaxisType, nodeInfo.Additional.DefaultTaxisType);
                ControlUtils.SelectSingleItem(RblIsChannelAddable, nodeInfo.Additional.IsChannelAddable.ToString());
                ControlUtils.SelectSingleItem(RblIsContentAddable, nodeInfo.Additional.IsContentAddable.ToString());

                TbImageUrl.Text = nodeInfo.ImageUrl;

                TbContent.SetParameters(PublishmentSystemInfo, NodeAttribute.Content, nodeInfo.Content);

                TbKeywords.Text = nodeInfo.Keywords;
                TbDescription.Text = nodeInfo.Description;

                //this.Content.PublishmentSystemID = base.PublishmentSystemID;
                //this.Content.Text = StringUtility.TextEditorContentDecode(nodeInfo.Content, ConfigUtils.Instance.ApplicationPath, base.PublishmentSystemInfo.PublishmentSystemUrl);
            }
            else
            {
                CacAttributes.Attributes = new ExtendedAttributes(Request.Form);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            NodeInfo nodeInfo;
            try
            {

                nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemId, _nodeId);
                if (!nodeInfo.NodeIndexName.Equals(TbNodeIndexName.Text) && TbNodeIndexName.Text.Length != 0)
                {
                    var nodeIndexNameList = DataProvider.NodeDao.GetNodeIndexNameList(PublishmentSystemId);
                    if (nodeIndexNameList.IndexOf(TbNodeIndexName.Text) != -1)
                    {
                        FailMessage("栏目属性修改失败，栏目索引已存在！");
                        return;
                    }
                }

                if (nodeInfo.ContentModelPluginId != DdlContentModelPluginId.SelectedValue)
                {
                    nodeInfo.ContentModelPluginId = DdlContentModelPluginId.SelectedValue;
                    nodeInfo.ContentNum = DataProvider.ContentDao.GetCount(NodeManager.GetTableName(PublishmentSystemInfo, nodeInfo.ContentModelPluginId), nodeInfo.NodeId);
                }

                nodeInfo.ContentRelatedPluginIds = ControlUtils.GetSelectedListControlValueCollection(CblContentRelatedPluginIds);

                TbFilePath.Text = TbFilePath.Text.Trim();
                if (!nodeInfo.FilePath.Equals(TbFilePath.Text) && TbFilePath.Text.Length != 0)
                {
                    if (!DirectoryUtils.IsDirectoryNameCompliant(TbFilePath.Text))
                    {
                        FailMessage("栏目页面路径不符合系统要求！");
                        return;
                    }

                    if (PathUtils.IsDirectoryPath(TbFilePath.Text))
                    {
                        TbFilePath.Text = PageUtils.Combine(TbFilePath.Text, "index.html");
                    }

                    var filePathArrayList = DataProvider.NodeDao.GetAllFilePathByPublishmentSystemId(PublishmentSystemId);
                    if (filePathArrayList.IndexOf(TbFilePath.Text) != -1)
                    {
                        FailMessage("栏目修改失败，栏目页面路径已存在！");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TbChannelFilePathRule.Text))
                {
                    var filePathRule = TbChannelFilePathRule.Text.Replace("|", string.Empty);
                    if (!DirectoryUtils.IsDirectoryNameCompliant(filePathRule))
                    {
                        FailMessage("栏目页面命名规则不符合系统要求！");
                        return;
                    }
                    if (PathUtils.IsDirectoryPath(filePathRule))
                    {
                        FailMessage("栏目页面命名规则必须包含生成文件的后缀！");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TbContentFilePathRule.Text))
                {
                    var filePathRule = TbContentFilePathRule.Text.Replace("|", string.Empty);
                    if (!DirectoryUtils.IsDirectoryNameCompliant(filePathRule))
                    {
                        FailMessage("内容页面命名规则不符合系统要求！");
                        return;
                    }
                    if (PathUtils.IsDirectoryPath(filePathRule))
                    {
                        FailMessage("内容页面命名规则必须包含生成文件的后缀！");
                        return;
                    }
                }

                var extendedAttributes = new ExtendedAttributes();
                var relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(PublishmentSystemId, _nodeId);
                var styleInfoList = TableStyleManager.GetTableStyleInfoList(DataProvider.NodeDao.TableName, relatedIdentities);
                BackgroundInputTypeParser.SaveAttributes(extendedAttributes, PublishmentSystemInfo, styleInfoList, Request.Form, null);
                var attributes = extendedAttributes.ToNameValueCollection();
                foreach (string key in attributes)
                {
                    nodeInfo.Additional.Set(key, attributes[key]);
                }

                nodeInfo.NodeName = TbNodeName.Text;
                nodeInfo.NodeIndexName = TbNodeIndexName.Text;
                nodeInfo.FilePath = TbFilePath.Text;
                nodeInfo.ChannelFilePathRule = TbChannelFilePathRule.Text;
                nodeInfo.ContentFilePathRule = TbContentFilePathRule.Text;

                var list = new ArrayList();
                foreach (ListItem item in CblNodeGroupNameCollection.Items)
                {
                    if (item.Selected)
                    {
                        list.Add(item.Value);
                    }
                }
                nodeInfo.NodeGroupNameCollection = TranslateUtils.ObjectCollectionToString(list);
                nodeInfo.ImageUrl = TbImageUrl.Text;
                nodeInfo.Content = ContentUtility.TextEditorContentEncode(PublishmentSystemInfo, Request.Form[NodeAttribute.Content]);

                nodeInfo.Keywords = TbKeywords.Text;
                nodeInfo.Description = TbDescription.Text;

                nodeInfo.Additional.IsChannelAddable = TranslateUtils.ToBool(RblIsChannelAddable.SelectedValue);
                nodeInfo.Additional.IsContentAddable = TranslateUtils.ToBool(RblIsContentAddable.SelectedValue);

                nodeInfo.LinkUrl = TbLinkUrl.Text;
                nodeInfo.LinkType = DdlLinkType.SelectedValue;
                nodeInfo.Additional.DefaultTaxisType = ETaxisTypeUtils.GetValue(ETaxisTypeUtils.GetEnumType(DdlTaxisType.SelectedValue));
                nodeInfo.ChannelTemplateId = DdlChannelTemplateId.Items.Count > 0 ? TranslateUtils.ToInt(DdlChannelTemplateId.SelectedValue) : 0;
                nodeInfo.ContentTemplateId = DdlContentTemplateId.Items.Count > 0 ? TranslateUtils.ToInt(DdlContentTemplateId.SelectedValue) : 0;

                DataProvider.NodeDao.UpdateNodeInfo(nodeInfo);
            }
            catch (Exception ex)
            {
                FailMessage(ex, $"栏目修改失败：{ex.Message}");
                LogUtils.AddSystemErrorLog(ex);
                return;
            }

            CreateManager.CreateChannel(PublishmentSystemId, nodeInfo.NodeId);

            Body.AddSiteLog(PublishmentSystemId, "修改栏目", $"栏目:{TbNodeName.Text}");

            SuccessMessage("栏目修改成功！");
            PageUtils.Redirect(ReturnUrl);
        }

        public string ReturnUrl { get; private set; }
    }
}