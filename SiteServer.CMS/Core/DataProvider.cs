using System;
using System.Collections.Generic;
using SiteServer.CMS.Data;
using SiteServer.CMS.Provider;
using SiteServer.Plugin.Apis;
using SiteServer.Utils;
using SiteServer.Utils.Model.Enumerations;
using ContentDao = SiteServer.CMS.Provider.ContentDao;
using PublishmentSystemLogDao = SiteServer.CMS.Provider.PublishmentSystemLogDao;

namespace SiteServer.CMS.Core
{
    public class DataProvider
    {
        private static IDataApi _dataApi;
        public static IDataApi DataApi
        {
            get
            {
                if (_dataApi != null) return _dataApi;

                switch (WebConfigUtils.DatabaseType)
                {
                    case EDatabaseType.MySql:
                        _dataApi = new Data.MySql();
                        break;
                    case EDatabaseType.SqlServer:
                        _dataApi = new SqlServer();
                        break;
                    case EDatabaseType.PostgreSql:
                        _dataApi = new PostgreSql();
                        break;
                    case EDatabaseType.Oracle:
                        _dataApi = new Data.Oracle();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return _dataApi;
            }
        }

        private static ContentCheckDao _contentCheckDao;
        public static ContentCheckDao ContentCheckDao => _contentCheckDao ?? (_contentCheckDao = new ContentCheckDao());

        private static CountDao _countDao;
        internal static CountDao CountDao => _countDao ?? (_countDao = new CountDao());

        private static DatabaseDao _databaseDao;
        public static DatabaseDao DatabaseDao => _databaseDao ?? (_databaseDao = new DatabaseDao());

        private static DbCacheDao _dbCacheDao;
        internal static DbCacheDao DbCacheDao => _dbCacheDao ?? (_dbCacheDao = new DbCacheDao());

        private static DiggDao _diggDao;
        public static DiggDao DiggDao => _diggDao ?? (_diggDao = new DiggDao());

        private static TableCollectionDao _tableCollectionDao;
        public static TableCollectionDao TableCollectionDao => _tableCollectionDao ?? (_tableCollectionDao = new TableCollectionDao());

        private static TableMatchDao _tableMatchDao;
        public static TableMatchDao TableMatchDao => _tableMatchDao ?? (_tableMatchDao = new TableMatchDao());

        private static TableMetadataDao _tableMetadataDao;
        public static TableMetadataDao TableMetadataDao => _tableMetadataDao ?? (_tableMetadataDao = new TableMetadataDao());

        private static TableStyleDao _tableStyleDao;
        public static TableStyleDao TableStyleDao => _tableStyleDao ?? (_tableStyleDao = new TableStyleDao());

        private static TableStyleItemDao _tableStyleItemDao;
        public static TableStyleItemDao TableStyleItemDao => _tableStyleItemDao ?? (_tableStyleItemDao = new TableStyleItemDao());

        private static TagDao _tagDao;
        public static TagDao TagDao => _tagDao ?? (_tagDao = new TagDao());

        private static UserDao _userDao;
        public static UserDao UserDao => _userDao ?? (_userDao = new UserDao());

        private static UserLogDao _userLogDao;
        public static UserLogDao UserLogDao => _userLogDao ?? (_userLogDao = new UserLogDao());

        private static ConfigDao _configDao;
        public static ConfigDao ConfigDao => _configDao ?? (_configDao = new ConfigDao());

        private static ErrorLogDao _errorLogDao;
        public static ErrorLogDao ErrorLogDao => _errorLogDao ?? (_errorLogDao = new ErrorLogDao());

        private static LogDao _logDao;
        public static LogDao LogDao => _logDao ?? (_logDao = new LogDao());

        private static RecordDao _recordDao;
        public static RecordDao RecordDao => _recordDao ?? (_recordDao = new RecordDao());

        private static PermissionsInRolesDao _permissionsInRolesDao;
        public static PermissionsInRolesDao PermissionsInRolesDao => _permissionsInRolesDao ?? (_permissionsInRolesDao = new PermissionsInRolesDao());

        private static AdAreaDao _adAreaDao;
        public static AdAreaDao AdAreaDao => _adAreaDao ?? (_adAreaDao = new AdAreaDao());

        private static AdMaterialDao _adMaterialDao;
        public static AdMaterialDao AdMaterialDao => _adMaterialDao ?? (_adMaterialDao = new AdMaterialDao());

        private static AdministratorDao _administratorDao;
        public static AdministratorDao AdministratorDao => _administratorDao ?? (_administratorDao = new AdministratorDao());

        private static AdministratorsInRolesDao _administratorsInRolesDao;
        public static AdministratorsInRolesDao AdministratorsInRolesDao => _administratorsInRolesDao ?? (_administratorsInRolesDao = new AdministratorsInRolesDao());

        private static RoleDao _roleDao;
        public static RoleDao RoleDao => _roleDao ?? (_roleDao = new RoleDao());

        private static AdvDao _advDao;
        public static AdvDao AdvDao => _advDao ?? (_advDao = new AdvDao());

        private static AdvertisementDao _advertisementDao;
        public static AdvertisementDao AdvertisementDao => _advertisementDao ?? (_advertisementDao = new AdvertisementDao());

        private static AreaDao _areaDao;
        public static AreaDao AreaDao => _areaDao ?? (_areaDao = new AreaDao());

        private static CommentDao _commentDao;
        public static CommentDao CommentDao => _commentDao ?? (_commentDao = new CommentDao());

        private static ContentDao _contentDao;
        public static ContentDao ContentDao => _contentDao ?? (_contentDao = new ContentDao());

        private static ContentGroupDao _contentGroupDao;
        public static ContentGroupDao ContentGroupDao => _contentGroupDao ?? (_contentGroupDao = new ContentGroupDao());

        private static DepartmentDao _departmentDao;
        public static DepartmentDao DepartmentDao => _departmentDao ?? (_departmentDao = new DepartmentDao());

        private static GatherDatabaseRuleDao _gatherDatabaseRuleDao;
        public static GatherDatabaseRuleDao GatherDatabaseRuleDao => _gatherDatabaseRuleDao ?? (_gatherDatabaseRuleDao = new GatherDatabaseRuleDao());

        private static GatherFileRuleDao _gatherFileRuleDao;
        public static GatherFileRuleDao GatherFileRuleDao => _gatherFileRuleDao ?? (_gatherFileRuleDao = new GatherFileRuleDao());

        private static GatherRuleDao _gatherRuleDao;
        public static GatherRuleDao GatherRuleDao => _gatherRuleDao ?? (_gatherRuleDao = new GatherRuleDao());

        private static InnerLinkDao _innerLinkDao;
        public static InnerLinkDao InnerLinkDao => _innerLinkDao ?? (_innerLinkDao = new InnerLinkDao());

        private static KeywordDao _keywordDao;
        public static KeywordDao KeywordDao => _keywordDao ?? (_keywordDao = new KeywordDao());

        private static NodeDao _nodeDao;
        public static NodeDao NodeDao => _nodeDao ?? (_nodeDao = new NodeDao());

        private static NodeGroupDao _nodeGroupDao;
        public static NodeGroupDao NodeGroupDao => _nodeGroupDao ?? (_nodeGroupDao = new NodeGroupDao());

        private static PhotoDao _photoDao;
        public static PhotoDao PhotoDao => _photoDao ?? (_photoDao = new PhotoDao());

        private static PluginConfigDao _pluginConfigDao;
        public static PluginConfigDao PluginConfigDao => _pluginConfigDao ?? (_pluginConfigDao = new PluginConfigDao());

        private static PluginDao _pluginDao;
        public static PluginDao PluginDao => _pluginDao ?? (_pluginDao = new PluginDao());

        private static PublishmentSystemDao _publishmentSystemDao;
        public static PublishmentSystemDao PublishmentSystemDao => _publishmentSystemDao ?? (_publishmentSystemDao = new PublishmentSystemDao());

        private static PublishmentSystemLogDao _publishmentSystemLogDao;
        public static PublishmentSystemLogDao PublishmentSystemLogDao => _publishmentSystemLogDao ?? (_publishmentSystemLogDao = new PublishmentSystemLogDao());

        private static RelatedFieldDao _relatedFieldDao;
        public static RelatedFieldDao RelatedFieldDao => _relatedFieldDao ?? (_relatedFieldDao = new RelatedFieldDao());

        private static RelatedFieldItemDao _relatedFieldItemDao;
        public static RelatedFieldItemDao RelatedFieldItemDao => _relatedFieldItemDao ?? (_relatedFieldItemDao = new RelatedFieldItemDao());

        private static SeoMetaDao _seoMetaDao;
        public static SeoMetaDao SeoMetaDao => _seoMetaDao ?? (_seoMetaDao = new SeoMetaDao());

        private static SeoMetasInNodesDao _seoMetasInNodesDao;
        public static SeoMetasInNodesDao SeoMetasInNodesDao => _seoMetasInNodesDao ?? (_seoMetasInNodesDao = new SeoMetasInNodesDao());

        private static StarDao _starDao;
        public static StarDao StarDao => _starDao ?? (_starDao = new StarDao());

        private static StarSettingDao _starSettingDao;
        public static StarSettingDao StarSettingDao => _starSettingDao ?? (_starSettingDao = new StarSettingDao());

        private static SystemPermissionsDao _systemPermissionsDao;
        public static SystemPermissionsDao SystemPermissionsDao => _systemPermissionsDao ?? (_systemPermissionsDao = new SystemPermissionsDao());

        private static TagStyleDao _tagStyleDao;
        public static TagStyleDao TagStyleDao => _tagStyleDao ?? (_tagStyleDao = new TagStyleDao());

        private static TemplateDao _templateDao;
        public static TemplateDao TemplateDao => _templateDao ?? (_templateDao = new TemplateDao());

        private static TemplateLogDao _templateLogDao;
        public static TemplateLogDao TemplateLogDao => _templateLogDao ?? (_templateLogDao = new TemplateLogDao());

        private static TemplateMatchDao _templateMatchDao;
        public static TemplateMatchDao TemplateMatchDao => _templateMatchDao ?? (_templateMatchDao = new TemplateMatchDao());

        private static TrackingDao _trackingDao;
        public static TrackingDao TrackingDao => _trackingDao ?? (_trackingDao = new TrackingDao());

        public static List<DataProviderBase> AllProviders => new List<DataProviderBase>
        {
            AdministratorsInRolesDao,
            RoleDao,
            AdAreaDao,
            AdMaterialDao,
            AdvDao,
            AdvertisementDao,
            CommentDao,
            ContentDao,
            ContentGroupDao,
            GatherDatabaseRuleDao,
            GatherFileRuleDao,
            GatherRuleDao,
            InnerLinkDao,
            KeywordDao,
            NodeDao,
            NodeGroupDao,
            PhotoDao,
            PluginConfigDao,
            PluginDao,
            PublishmentSystemDao,
            PublishmentSystemLogDao,
            RelatedFieldDao,
            RelatedFieldItemDao,
            SeoMetaDao,
            SeoMetasInNodesDao,
            StarDao,
            StarSettingDao,
            SystemPermissionsDao,
            TagStyleDao,
            TemplateDao,
            TemplateLogDao,
            TemplateMatchDao,
            TrackingDao
        };
    }
}
