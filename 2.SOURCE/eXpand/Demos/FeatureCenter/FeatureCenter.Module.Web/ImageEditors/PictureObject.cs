﻿using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Web.ImageEditors {
    [DisplayFeatureModel("PictureObject_ListView", "Thumbnail")]
    [DefaultProperty("Title")]
    [AdditionalViewControlsRule(Module.Captions.ViewMessage + " " + Captions.HeaderThumbnail, "1=1", "1=1",
        Captions.ViewMessageThumbnail, Position.Bottom, ViewType = ViewType.ListView, Nesting = Nesting.Root)]
    [AdditionalViewControlsRule(Module.Captions.Header + " " + Captions.HeaderThumbnail, "1=1", "1=1",
        Captions.HeaderThumbnail, Position.Top, ViewType = ViewType.ListView, Nesting = Nesting.Root)]
    public class PictureObject : BaseObject, IPictureItem {
        PictureMasterObject _horizantalMasterObject;
        PictureMasterObject _horizontalMasterObjectStyleModified;
        PictureMasterObject _verticalMasterObject;
        private Image _cover;
        private string _title;

        public PictureObject(Session session)
            : base(session) {
        }
        private PictureMasterObject _verticalMasterObjectWithNoImage;


        [VisibleInListView(false)]
        [Association("MasterObject-VerticalPicObjectsWithNoImage")]
        public PictureMasterObject VerticalMasterObjectWithNoImage {
            get { return _verticalMasterObjectWithNoImage; }
            set { SetPropertyValue("VerticalMasterObjectWithNoImage", ref _verticalMasterObjectWithNoImage, value); }
        }

        private PictureMasterObject _horizontalMasterObjectWithNoImage;
        [VisibleInListView(false)]
        [Association("MasterObject-HorizontalPicObjectsWithNoImage")]
        public PictureMasterObject HorizontalMasterObjectWithNoImage {
            get { return _horizontalMasterObjectWithNoImage; }
            set { SetPropertyValue("HorizontalPicObjectWithNoImage", ref _horizontalMasterObjectWithNoImage, value); }
        }
        [VisibleInListView(false)]
        [Association("MasterObject-HorizontalPictureObjects")]
        public PictureMasterObject HorizantalMasterObject {
            get { return _horizantalMasterObject; }
            set { SetPropertyValue("PropertyName", ref _horizantalMasterObject, value); }
        }
        [VisibleInListView(false)]
        [Association("MasterObject-HorizontalPictureObjectsStyleModified")]
        public PictureMasterObject HorizontalMasterObjectStyleModified {
            get { return _horizontalMasterObjectStyleModified; }
            set { SetPropertyValue("HorizontalMasterObjectStyleModified", ref _horizontalMasterObjectStyleModified, value); }
        }
        [VisibleInListView(false)]
        [Association("MasterObject-VerticalPictureObjects")]
        public PictureMasterObject VerticalMasterObject {
            get { return _verticalMasterObject; }
            set { SetPropertyValue("VerticalMasterObject", ref _verticalMasterObject, value); }
        }
        private PictureMasterObject _verticalMasterObjectStyleModified;

        [VisibleInListView(false)]
        [Association("MasterObject-VerticalPicObjects")]
        public PictureMasterObject VerticalMasterObjectStyleModified {
            get { return _verticalMasterObjectStyleModified; }
            set { SetPropertyValue("VerticalMasterObjectStyleModified", ref _verticalMasterObjectStyleModified, value); }
        }
        public string Title {
            get { return _title; }
            set { SetPropertyValue("Title", ref _title, value); }
        }
        private string _subTitle;
        public string SubTitle {
            get { return _subTitle; }
            set { SetPropertyValue("SubTitle", ref _subTitle, value); }
        }
        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited), ValueConverter(typeof(ImageValueConverter))]
        public Image Cover {
            get { return _cover; }
            set { SetPropertyValue("Cover", ref _cover, value); }
        }
        #region IPictureItem Members
        string IPictureItem.ID => Oid.ToString();

        Image IPictureItem.Image => Cover;


        private string _imagePath;
        [Browsable(false)]
        public string ImagePath {
            get { return _imagePath; }
            set { SetPropertyValue("ImagePath", ref _imagePath, value); }
        }

        #endregion
    }
}
