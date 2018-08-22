using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects;
using Xpand.Persistent.Base.ModelDifference;

namespace Xpand.ExpressApp.ModelDifference.DataStore.Queries {
    public class QueryUserModelDifferenceObject : QueryDifferenceObject<UserModelDifferenceObject> {
        public QueryUserModelDifferenceObject(Session session)
            : base(session) {
        }

        private static ContainsOperator UsersContainsOperator {
            get {
                var xpBaseObject = (SecuritySystem.CurrentUser) as XPBaseObject;
                if (xpBaseObject != null && XafTypesInfo.Instance.FindTypeInfo(typeof(UserModelDifferenceObject)).FindMember("Users")!=null) {
                    XPMemberInfo mi = xpBaseObject.ClassInfo.KeyProperty;
                    return new ContainsOperator("Users", new BinaryOperator(mi.Name, mi.GetValue(SecuritySystem.CurrentUser)));
                }
                return null;
            }
        }

        public override IQueryable<UserModelDifferenceObject> GetActiveModelDifferences(string applicationName, string name, DeviceCategory deviceCategory = DeviceCategory.All) {
            return new XPCollection<UserModelDifferenceObject>(Session, new GroupOperator(UsersContainsOperator,
                    new GroupOperator(new BinaryOperator("PersistentApplication.UniqueName", applicationName),
                        new BinaryOperator("Disabled", false),
                        new BinaryOperator(nameof(ModelDifferenceObject.DeviceCategory), deviceCategory))),
                new SortProperty("CombineOrder", SortingDirection.Ascending)).AsQueryable();
        }

        public override UserModelDifferenceObject GetActiveModelDifference(string applicationName, string name,DeviceCategory deviceCategory = DeviceCategory.All) {
            return GetActiveModelDifferences(applicationName, name,deviceCategory).FirstOrDefault();
        }

    }
}