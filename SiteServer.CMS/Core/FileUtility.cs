using System;
using SiteServer.Utils;
using SiteServer.Utils.Model;
using SiteServer.CMS.Model;
using System.Collections.Generic;
using SiteServer.Utils.Images;
using SiteServer.Utils.Model.Enumerations;

namespace SiteServer.CMS.Core
{
    public class FileUtility
    {
        private FileUtility()
        {
        }

        public static void AddWaterMark(PublishmentSystemInfo publishmentSystemInfo, string imagePath)
        {
            try
            {
                var fileExtName = PathUtils.GetExtension(imagePath);
                if (EFileSystemTypeUtils.IsImage(fileExtName))
                {
                    if (publishmentSystemInfo.Additional.IsWaterMark)
                    {
                        if (publishmentSystemInfo.Additional.IsImageWaterMark)
                        {
                            if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.WaterMarkImagePath))
                            {
                                ImageUtils.AddImageWaterMark(imagePath, PathUtility.MapPath(publishmentSystemInfo, publishmentSystemInfo.Additional.WaterMarkImagePath), publishmentSystemInfo.Additional.WaterMarkPosition, publishmentSystemInfo.Additional.WaterMarkTransparency, publishmentSystemInfo.Additional.WaterMarkMinWidth, publishmentSystemInfo.Additional.WaterMarkMinHeight);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.WaterMarkFormatString))
                            {
                                var now = DateTime.Now;
                                ImageUtils.AddTextWaterMark(imagePath, string.Format(publishmentSystemInfo.Additional.WaterMarkFormatString, DateUtils.GetDateString(now), DateUtils.GetTimeString(now)), publishmentSystemInfo.Additional.WaterMarkFontName, publishmentSystemInfo.Additional.WaterMarkFontSize, publishmentSystemInfo.Additional.WaterMarkPosition, publishmentSystemInfo.Additional.WaterMarkTransparency, publishmentSystemInfo.Additional.WaterMarkMinWidth, publishmentSystemInfo.Additional.WaterMarkMinHeight);
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void MoveFile(PublishmentSystemInfo sourcePublishmentSystemInfo, PublishmentSystemInfo destPublishmentSystemInfo, string relatedUrl)
        {
            if (!string.IsNullOrEmpty(relatedUrl))
            {
                var sourceFilePath = PathUtility.MapPath(sourcePublishmentSystemInfo, relatedUrl);
                var descFilePath = PathUtility.MapPath(destPublishmentSystemInfo, relatedUrl);
                if (FileUtils.IsFileExists(sourceFilePath))
                {
                    FileUtils.MoveFile(sourceFilePath, descFilePath, false);
                }
            }
        }

        public static void MoveFileByContentInfo(PublishmentSystemInfo sourcePublishmentSystemInfo, PublishmentSystemInfo destPublishmentSystemInfo, ContentInfo contentInfo)
        {
            if (contentInfo == null || sourcePublishmentSystemInfo.PublishmentSystemId == destPublishmentSystemInfo.PublishmentSystemId) return;

            try
            {
                var fileUrls = new List<string>
                {
                    contentInfo.GetString(BackgroundContentAttribute.ImageUrl),
                    contentInfo.GetString(BackgroundContentAttribute.VideoUrl),
                    contentInfo.GetString(BackgroundContentAttribute.FileUrl)
                };

                foreach (var url in TranslateUtils.StringCollectionToStringList(contentInfo.GetString(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl))))
                {
                    if (!fileUrls.Contains(url))
                    {
                        fileUrls.Add(url);
                    }
                }
                foreach (var url in TranslateUtils.StringCollectionToStringList(contentInfo.GetString(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.VideoUrl))))
                {
                    if (!fileUrls.Contains(url))
                    {
                        fileUrls.Add(url);
                    }
                }
                foreach (var url in TranslateUtils.StringCollectionToStringList(contentInfo.GetString(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl))))
                {
                    if (!fileUrls.Contains(url))
                    {
                        fileUrls.Add(url);
                    }
                }
                foreach (var url in RegexUtils.GetOriginalImageSrcs(contentInfo.GetString(BackgroundContentAttribute.Content)))
                {
                    if (!fileUrls.Contains(url))
                    {
                        fileUrls.Add(url);
                    }
                }
                foreach (var url in RegexUtils.GetOriginalLinkHrefs(contentInfo.GetString(BackgroundContentAttribute.Content)))
                {
                    if (!fileUrls.Contains(url) && PageUtils.IsVirtualUrl(url))
                    {
                        fileUrls.Add(url);
                    }
                }

                foreach (var fileUrl in fileUrls)
                {
                    if (!string.IsNullOrEmpty(fileUrl) && PageUtility.IsVirtualUrl(fileUrl))
                    {
                        MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, fileUrl);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void MoveFileByVirtaulUrl(PublishmentSystemInfo sourcePublishmentSystemInfo, PublishmentSystemInfo destPublishmentSystemInfo, string fileVirtaulUrl)
        {
            if (string.IsNullOrEmpty(fileVirtaulUrl) || sourcePublishmentSystemInfo.PublishmentSystemId == destPublishmentSystemInfo.PublishmentSystemId) return;

            try
            {
                if (PageUtility.IsVirtualUrl(fileVirtaulUrl))
                {
                    MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, fileVirtaulUrl);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
