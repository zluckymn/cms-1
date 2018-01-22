﻿using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.BackgroundPages.Ajax;
using SiteServer.BackgroundPages.Core;
using SiteServer.CMS.Core.Create;

namespace SiteServer.BackgroundPages.Cms
{
    public class ModalProgressBar : BasePageCms
    {
        protected override bool IsSinglePage => true;

        public Literal LtlScripts;

        public static string GetOpenWindowStringWithCreateContentsOneByOne(int publishmentSystemId, int nodeId)
        {
            return LayerUtils.GetOpenScriptWithCheckBoxValue("生成内容页",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"PublishmentSystemID", publishmentSystemId.ToString()},
                    {"NodeID", nodeId.ToString()},
                    {"CreateContentsOneByOne", true.ToString()}
                }), "ContentIDCollection", "请选择需要生成的内容！", 500, 360);
        }

        public static string GetOpenWindowStringWithCreateByTemplate(int publishmentSystemId, int templateId)
        {
            return LayerUtils.GetOpenScript("生成页面",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"PublishmentSystemID", publishmentSystemId.ToString()},
                    {"templateID", templateId.ToString()},
                    {"CreateByTemplate", true.ToString()}
                }), 500, 360);
        }

        public static string GetRedirectUrlStringWithCreateChannelsOneByOne(int publishmentSystemId,
            string channelIdCollection, string isIncludeChildren, string isCreateContents)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"CreateChannelsOneByOne", true.ToString()},
                {"ChannelIDCollection", channelIdCollection},
                {"IsIncludeChildren", isIncludeChildren},
                {"IsCreateContents", isCreateContents}
            });
        }

        public static string GetRedirectUrlStringWithCreateContentsOneByOne(int publishmentSystemId, int nodeId,
            string contentIdCollection)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"NodeID", nodeId.ToString()},
                {"CreateContentsOneByOne", true.ToString()},
                {"ContentIDCollection", contentIdCollection}
            });
        }

        public static string GetRedirectUrlStringWithCreateByIDsCollection(int publishmentSystemId, string idsCollection)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"CreateByIDsCollection", true.ToString()},
                {"IDsCollection", idsCollection}
            });
        }

        public static string GetRedirectUrlStringWithGather(int publishmentSystemId, string gatherRuleNameCollection)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"Gather", true.ToString()},
                {"GatherRuleNameCollection", gatherRuleNameCollection}
            });
        }

        public static string GetOpenWindowStringWithGather(int publishmentSystemId)
        {
            return LayerUtils.GetOpenScriptWithCheckBoxValue("采集内容",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"PublishmentSystemID", publishmentSystemId.ToString()},
                    {"Gather", true.ToString()}
                }), "GatherRuleNameCollection", "请选择需要开始采集的采集规则名称!", 660, 360);
        }

        public static string GetRedirectUrlStringWithGatherDatabase(int publishmentSystemId,
            string gatherRuleNameCollection)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"GatherDatabase", true.ToString()},
                {"GatherRuleNameCollection", gatherRuleNameCollection}
            });
        }

        public static string GetOpenWindowStringWithGatherDatabase(int publishmentSystemId)
        {
            return LayerUtils.GetOpenScriptWithCheckBoxValue("数据库采集",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"PublishmentSystemID", publishmentSystemId.ToString()},
                    {"GatherDatabase", true.ToString()}
                }), "GatherRuleNameCollection", "请选择需要开始采集的采集规则名称!", 660, 360);
        }

        public static string GetRedirectUrlStringWithGatherFile(int publishmentSystemId, string gatherRuleNameCollection)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"GatherFile", true.ToString()},
                {"GatherRuleNameCollection", gatherRuleNameCollection}
            });
        }

        public static string GetOpenWindowStringWithGatherFile(int publishmentSystemId)
        {
            return LayerUtils.GetOpenScriptWithCheckBoxValue("文件采集",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"PublishmentSystemID", publishmentSystemId.ToString()},
                    {"GatherFile", true.ToString()}
                }), "GatherRuleNameCollection", "请选择需要开始采集的采集规则名称!", 660, 360);
        }

        public static string GetOpenWindowStringWithSiteTemplateDownload(string downloadUrl)
        {
            return LayerUtils.GetOpenScript("下载在线模板",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"SiteTemplateDownload", true.ToString()},
                    {"DownloadUrl", TranslateUtils.EncryptStringBySecretKey(downloadUrl)}
                }), 460, 360);
        }

        public static string GetRedirectUrlStringWithSiteTemplateDownload(string downloadUrl)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"SiteTemplateDownload", true.ToString()},
                {"DownloadUrl", TranslateUtils.EncryptStringBySecretKey(downloadUrl)}
            });
        }

        public static string GetOpenWindowStringWithSiteTemplateZip(string directoryName)
        {
            return LayerUtils.GetOpenScript("站点模板压缩",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"SiteTemplateZip", true.ToString()},
                    {"DirectoryName", directoryName}
                }), 460, 360);
        }

        public static string GetOpenWindowStringWithSiteTemplateUnZip(string fileName)
        {
            return LayerUtils.GetOpenScript("站点模板解压",
                PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
                {
                    {"SiteTemplateUnZip", true.ToString()},
                    {"FileName", fileName}
                }), 460, 360);
        }

        public static string GetRedirectUrlStringWithPluginDownload(string downloadUrl)
        {
            return PageUtils.GetCmsUrl(nameof(ModalProgressBar), new NameValueCollection
            {
                {"PluginDownload", true.ToString()},
                {"DownloadUrl", downloadUrl}
            });
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (Body.IsQueryExists("Gather") && Body.IsQueryExists("GatherRuleNameCollection"))
            {
                var userKeyPrefix = StringUtils.Guid();
                var parameters = AjaxGatherService.GetGatherParameters(PublishmentSystemId, Body.GetQueryString("GatherRuleNameCollection"), userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxGatherService.GetGatherUrl(), parameters, userKeyPrefix, AjaxGatherService.GetCountArrayUrl());
            }
            else if (Body.IsQueryExists("GatherDatabase") && Body.IsQueryExists("GatherRuleNameCollection"))
            {
                var userKeyPrefix = StringUtils.Guid();
                var parameters = AjaxGatherService.GetGatherDatabaseParameters(PublishmentSystemId,
                    Body.GetQueryString("GatherRuleNameCollection"), userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxGatherService.GetGatherDatabaseUrl(), parameters, userKeyPrefix, AjaxGatherService.GetCountArrayUrl());
            }
            else if (Body.IsQueryExists("GatherFile") && Body.IsQueryExists("GatherRuleNameCollection"))
            {
                var userKeyPrefix = StringUtils.Guid();
                var parameters = AjaxGatherService.GetGatherFileParameters(PublishmentSystemId,
                    Body.GetQueryString("GatherRuleNameCollection"), userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxGatherService.GetGatherFileUrl(), parameters,
                        userKeyPrefix, AjaxGatherService.GetCountArrayUrl());
            }
            //----------------------------------------------------------------------------------------//
            else if (Body.IsQueryExists("CreateChannelsOneByOne") && Body.IsQueryExists("ChannelIDCollection"))
            {
                foreach (var channelId in TranslateUtils.StringCollectionToIntList(Body.GetQueryString("ChannelIDCollection")))
                {
                    CreateManager.CreateChannel(PublishmentSystemId, channelId);
                }

                PageUtils.Redirect(ModalTipMessage.GetRedirectUrlString("已成功将栏目放入生成队列"));
            }
            else if (Body.IsQueryExists("CreateContentsOneByOne") && Body.IsQueryExists("NodeID") &&
                     Body.IsQueryExists("ContentIDCollection"))
            {
                foreach (var contentId in TranslateUtils.StringCollectionToIntList(Body.GetQueryString("ContentIDCollection")))
                {
                    CreateManager.CreateContent(PublishmentSystemId, Body.GetQueryInt("NodeID"),
                        contentId);
                }

                PageUtils.Redirect(ModalTipMessage.GetRedirectUrlString("已成功将内容放入生成队列"));
            }
            else if (Body.IsQueryExists("CreateByTemplate") && Body.IsQueryExists("templateID"))
            {
                CreateManager.CreateFile(PublishmentSystemId, Body.GetQueryInt("templateID"));

                PageUtils.Redirect(ModalTipMessage.GetRedirectUrlString("已成功将文件放入生成队列"));
            }
            else if (Body.IsQueryExists("CreateByIDsCollection") && Body.IsQueryExists("IDsCollection"))
            {
                foreach (var nodeIdContentId in
                    TranslateUtils.StringCollectionToStringCollection(Body.GetQueryString("IDsCollection")))
                {
                    var pair = nodeIdContentId.Split('_');
                    CreateManager.CreateContent(PublishmentSystemId, TranslateUtils.ToInt(pair[0]),
                        TranslateUtils.ToInt(pair[1]));
                }

                PageUtils.Redirect(ModalTipMessage.GetRedirectUrlString("已成功将文件放入生成队列"));
            }
            //---------------------------------------------------------------------------------------//
            else if (Body.IsQueryExists("SiteTemplateDownload"))
            {
                var userKeyPrefix = StringUtils.Guid();

                var downloadUrl = TranslateUtils.DecryptStringBySecretKey(Body.GetQueryString("DownloadUrl"));
                var directoryName = PathUtils.GetFileNameWithoutExtension(downloadUrl);

                var parameters = AjaxOtherService.GetSiteTemplateDownloadParameters(downloadUrl, directoryName, userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxOtherService.GetSiteTemplateDownloadUrl(), parameters, userKeyPrefix, AjaxOtherService.GetCountArrayUrl());
            }
            else if (Body.IsQueryExists("SiteTemplateZip"))
            {
                var userKeyPrefix = StringUtils.Guid();

                var parameters = AjaxOtherService.GetSiteTemplateZipParameters(Body.GetQueryString("DirectoryName"), userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxOtherService.GetSiteTemplateZipUrl(), parameters, userKeyPrefix, AjaxOtherService.GetCountArrayUrl());
            }
            else if (Body.IsQueryExists("SiteTemplateUnZip"))
            {
                var userKeyPrefix = StringUtils.Guid();

                var parameters = AjaxOtherService.GetSiteTemplateUnZipParameters(Body.GetQueryString("FileName"), userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxOtherService.GetSiteTemplateUnZipUrl(), parameters, userKeyPrefix, AjaxOtherService.GetCountArrayUrl());
            }
            //---------------------------------------------------------------------------------------//
            else if (Body.IsQueryExists("PluginDownload"))
            {
                var userKeyPrefix = StringUtils.Guid();

                var parameters = AjaxOtherService.GetPluginDownloadParameters(Body.GetQueryString("DownloadUrl"), userKeyPrefix);
                LtlScripts.Text =
                    AjaxManager.RegisterProgressTaskScript(AjaxOtherService.GetPluginDownloadUrl(), parameters, userKeyPrefix, AjaxOtherService.GetCountArrayUrl());
            }
        }
    }
}
