using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Phenix.Unity.UI;
using UnityEngine.EventSystems;
using Phenix.Unity.Utilities;

namespace Phenix.Unity.Editor
{
    public class RegisterUIControls
    {
        #region ------------------ 支持双击、长按的button --------------------        
        [MenuItem("GameObject/UI/Phenix/ButtonEx", false)]
        public static void RegisterButtonEx(MenuCommand menuCommand)
        {
            GameObject button = CreateButtonEx(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(button, menuCommand);
        }

        static GameObject CreateButtonEx(UIResources resources)
        {
            GameObject button = DefaultControls.CreateButton(UIDefaultControls.ConvertToDefaultResources(resources));
            button.name = "ButtonEx";
            button.transform.Find("Text").GetComponent<Text>().text = "ButtonEx";            
            button.AddComponent<DoubleClickable>();
            button.AddComponent<LongPressable>();
            return button;
        }
        #endregion

        #region -------------- 支持拖动、颜色渐变的image --------------

        [MenuItem("GameObject/UI/Phenix/ImageEx", false)]
        public static void RegisterImageEx(MenuCommand menuCommand)
        {
            GameObject image = CreateImageEx(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(image, menuCommand);
        }

        static GameObject CreateImageEx(UIResources resources)
        {
            GameObject image = DefaultControls.CreateImage(UIDefaultControls.ConvertToDefaultResources(resources));
            image.name = "ImageEx";                        
            image.AddComponent<UIDragable>();
            image.AddComponent<ColorGradient>();
            return image;
        }
        #endregion

        #region ---------------- 支持拖动、颜色渐变的raw image ----------------
        [MenuItem("GameObject/UI/Phenix/RawImageEx", false)]
        public static void RegisterRawImageEx(MenuCommand menuCommand)
        {
            GameObject rawImage = CreateRawImageEx(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(rawImage, menuCommand);
        }

        static GameObject CreateRawImageEx(UIResources resources)
        {
            GameObject rawImage = DefaultControls.CreateRawImage(UIDefaultControls.ConvertToDefaultResources(resources));
            rawImage.name = "RawImageEx";
            rawImage.AddComponent<UIDragable>();
            rawImage.AddComponent<ColorGradient>();
            return rawImage;
        }
        #endregion

        #region ---------------- 支持竖排、颜色渐变、描边、投影的Text ----------------
        [MenuItem("GameObject/UI/Phenix/TextEx", false)]
        public static void RegisterTextEx(MenuCommand menuCommand)
        {
            GameObject text = CreateTextEx(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(text, menuCommand);
        }

        static GameObject CreateTextEx(UIResources resources)
        {
            GameObject text = DefaultControls.CreateText(UIDefaultControls.ConvertToDefaultResources(resources));
            text.name = "TextEx";
            text.AddComponent<VerticalText>().enabled = false;
            text.AddComponent<ColorGradient>().enabled = false;
            text.AddComponent<Outline>().enabled = false;
            text.AddComponent<Shadow>().enabled = false;
            return text;
        }
        #endregion

        #region ---------------- 摇杆JoyStick ----------------
        [MenuItem("GameObject/UI/Phenix/JoyStick", false)]
        public static void RegisterJoyStick(MenuCommand menuCommand)
        {
            GameObject joyStick = CreateJoyStick(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(joyStick, menuCommand);

            // 以下三行必须在此执行，若放在CreateJoyStick中会因为新建对象层级未绑定以至canvas为null
            UIDragable dragable = joyStick.GetComponentInChildren<UIDragable>();
            Canvas canvas = dragable.GetComponentInParent<Canvas>();
            dragable.rect = canvas.GetComponent<RectTransform>();
        }

        static GameObject CreateJoyStick(UIResources resources)
        {
            GameObject stickPanel = DefaultControls.CreateImage(UIDefaultControls.ConvertToDefaultResources(resources));
            stickPanel.name = "StickPanel";

            GameObject stickSlider = CreateImageEx(UIMenuTools.GetStandardResources());
            stickSlider.name = "StickSlider";            
            stickSlider.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            JoyStick joyStick = stickSlider.AddComponent<JoyStick>();
            joyStick.dragable = stickSlider.GetComponent<UIDragable>();
            joyStick.panel = stickPanel.transform as RectTransform;
            joyStick.slider = stickSlider.transform as RectTransform;

            stickSlider.transform.parent = stickPanel.transform;

            return stickPanel;
        }
        #endregion

        #region ---------------- 雷达图RadarChart ----------------
        [MenuItem("GameObject/UI/Phenix/RadarChart", false)]
        public static void RegisterRadarChart(MenuCommand menuCommand)
        {
            GameObject radarChart = CreateRadarChart(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(radarChart, menuCommand);
        }

        static GameObject CreateRadarChart(UIResources resources)
        {
            GameObject radarPanel = DefaultControls.CreateImage(UIDefaultControls.ConvertToDefaultResources(resources));
            radarPanel.name = "RadarPanel";

            GameObject radarChart = new GameObject("RadarChart");
            RadarChart radar = radarChart.AddComponent<RadarChart>();
            radar.color = Color.red;
            radarChart.transform.parent = radarPanel.transform;            

            return radarPanel;
        }
        #endregion

        #region ---------------- 数字Image ----------------
        [MenuItem("GameObject/UI/Phenix/ImageNumbers", false)]
        public static void RegisterImageNumbers(MenuCommand menuCommand)
        {
            GameObject imageNumbers = CreateImageNumbers(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(imageNumbers, menuCommand);
        }

        static GameObject CreateImageNumbers(UIResources resources)
        {
            GameObject go = new GameObject("ImageNumbers");
            var layout = go.AddComponent<HorizontalLayoutGroup>();
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;
            go.AddComponent<CanvasGroup>();
            go.AddComponent<NumFont>();

            return go;
        }
        #endregion

        #region ---------------- 角色面板 ----------------
        [MenuItem("GameObject/UI/Phenix/CharacterPanel", false)]
        public static void RegisterCharacterPanel(MenuCommand menuCommand)
        {
            GameObject characterPanel = CreateCharacterPanel(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(characterPanel, menuCommand);
        }

        static GameObject CreateCharacterPanel(UIResources resources)
        {
            GameObject characterPanel = new GameObject("CharacterPanel");
            characterPanel.AddComponent<Image>();
            characterPanel.AddComponent<Mask>();

            GameObject head = new GameObject("Head");
            head.AddComponent<Image>();

            GameObject hp = new GameObject("HP");
            hp.AddComponent<Image>();
            Slider hpSliderComp = hp.AddComponent<Slider>();

            GameObject hpSlider = new GameObject("HPSlider");
            hpSlider.AddComponent<Image>();
            hpSliderComp.fillRect = hpSlider.transform as RectTransform;
            hpSliderComp.direction = Slider.Direction.LeftToRight;
            hpSliderComp.minValue = 0;
            hpSliderComp.maxValue = 1;
            hpSliderComp.value = 1;
            hpSlider.transform.parent = hp.transform;

            GameObject mp = new GameObject("MP");
            mp.AddComponent<Image>();
            Slider mpSliderComp = mp.AddComponent<Slider>();

            GameObject mpSlider = new GameObject("MPSlider");
            mpSlider.AddComponent<Image>();
            mpSliderComp.fillRect = mpSlider.transform as RectTransform;
            mpSliderComp.direction = Slider.Direction.LeftToRight;
            mpSliderComp.minValue = 0;
            mpSliderComp.maxValue = 1;
            mpSliderComp.value = 1;
            mpSlider.transform.parent = mp.transform;

            head.transform.parent = characterPanel.transform;
            hp.transform.parent = characterPanel.transform;
            mp.transform.parent = characterPanel.transform;

            return characterPanel;
        }
        #endregion

        #region ---------------- 翻书页 ----------------
        [MenuItem("GameObject/UI/Phenix/FlipPage", false)]
        public static void RegisterFlipPage(MenuCommand menuCommand)
        {
            GameObject book = CreateFlipPage(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(book, menuCommand);
            // 以下必须在此执行，若放在CreateFlipPage中会因为新建对象层级未绑定以至canvas为null
            FlipPage flipPage = book.GetComponentInChildren<FlipPage>();
            Canvas canvas = book.GetComponentInParent<Canvas>();
            flipPage.canvas = canvas;

            flipPage.bookPanel = book.transform as RectTransform;
        }

        static GameObject CreateFlipPage(UIResources resources)
        {
            GameObject book = new GameObject("Book", typeof(RectTransform));
            FlipPage flipPage = book.AddComponent<FlipPage>();

            /*GameObject border = new GameObject("Border", typeof(Image));
            border.transform.parent = book.transform;
            border.GetComponent<Image>().raycastTarget = false;*/

            GameObject left = new GameObject("Left", typeof(Image));
            (left.transform as RectTransform).pivot = Vector2.zero;
            left.transform.parent = book.transform;
            left.GetComponent<Image>().raycastTarget = false;            
            left.GetComponent<Image>().enabled = true;
            left.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;

            GameObject right = new GameObject("Right", typeof(Image));
            (right.transform as RectTransform).pivot = Vector2.zero;
            right.transform.parent = book.transform;
            right.GetComponent<Image>().raycastTarget = false;            
            right.GetComponent<Image>().enabled = true;
            right.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;

            GameObject leftOnFlip = new GameObject("LeftOnFlip", typeof(Image), typeof(Mask));
            (leftOnFlip.transform as RectTransform).pivot = Vector2.zero;
            leftOnFlip.GetComponent<Mask>().showMaskGraphic = true;
            leftOnFlip.GetComponent<Image>().raycastTarget = false;
            leftOnFlip.transform.parent = book.transform;
            leftOnFlip.SetActive(false);
            GameObject shadowLTR = new GameObject("ShadowLTR", typeof(Image));
            shadowLTR.transform.parent = leftOnFlip.transform;
            shadowLTR.GetComponent<Image>().raycastTarget = false;
            shadowLTR.SetActive(false);

            GameObject rightOnFlip = new GameObject("RightOnFlip", typeof(Image), typeof(Mask));
            (rightOnFlip.transform as RectTransform).pivot = Vector2.zero;
            rightOnFlip.GetComponent<Mask>().showMaskGraphic = true;
            rightOnFlip.GetComponent<Image>().raycastTarget = false;
            rightOnFlip.transform.parent = book.transform;
            rightOnFlip.SetActive(false);
            GameObject shadow = new GameObject("Shadow", typeof(Image));
            shadow.transform.parent = rightOnFlip.transform;
            shadow.GetComponent<Image>().raycastTarget = false;
            shadow.SetActive(false);

            GameObject turnPageFlip = new GameObject("TurnPageFlip", typeof(Image), typeof(Mask));
            turnPageFlip.GetComponent<Mask>().showMaskGraphic = false;
            turnPageFlip.GetComponent<Image>().raycastTarget = false;
            turnPageFlip.transform.parent = book.transform;

            GameObject nextOnFlip = new GameObject("NextOnFlip", typeof(Image), typeof(Mask));
            nextOnFlip.GetComponent<Mask>().showMaskGraphic = false;
            nextOnFlip.GetComponent<Image>().raycastTarget = false;
            nextOnFlip.transform.parent = book.transform;            

            GameObject leftHotSpot = new GameObject("LeftHotSpot", typeof(Image), typeof(EventTrigger));
            leftHotSpot.GetComponent<Image>().color = new Vector4(1, 1, 1, 0);
            leftHotSpot.transform.parent = book.transform;            

            GameObject rightHotSpot = new GameObject("RightHotSpot", typeof(Image), typeof(EventTrigger));
            rightHotSpot.GetComponent<Image>().color = new Vector4(1, 1, 1, 0);
            rightHotSpot.transform.parent = book.transform;            
            
            flipPage.left = left.GetComponent<Image>();
            flipPage.right = right.GetComponent<Image>();
            flipPage.leftOnFlip = leftOnFlip.GetComponent<Image>();
            flipPage.rightOnFlip = rightOnFlip.GetComponent<Image>();
            flipPage.clippingPlane = turnPageFlip.GetComponent<Image>();
            flipPage.nextPageClip = nextOnFlip.GetComponent<Image>();
            flipPage.shadow = shadow.GetComponent<Image>();
            flipPage.shadowLTR = shadowLTR.GetComponent<Image>();
            flipPage.leftHotSpot = leftHotSpot.GetComponent<Image>();
            flipPage.rightHotSpot = rightHotSpot.GetComponent<Image>();

            return book;
        }
        #endregion

        #region ---------------- 横向滚动列表 ----------------
        [MenuItem("GameObject/UI/Phenix/HorizontalScrollView", false)]
        public static void RegisterHorizontalScrollView(MenuCommand menuCommand)
        {
            GameObject scroll = CreateHorizontalScrollView(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(scroll, menuCommand);
        }

        static GameObject CreateHorizontalScrollView(UIResources resources)
        {
            GameObject scroll = DefaultControls.CreateScrollView(UIDefaultControls.ConvertToDefaultResources(resources));
            scroll.name = "HScrollView";
            Transform toDel1 = scroll.transform.Find("Scrollbar Horizontal");
            Transform toDel2 = scroll.transform.Find("Scrollbar Vertical");
            GameObject.DestroyImmediate(toDel1.gameObject);
            GameObject.DestroyImmediate(toDel2.gameObject);

            RectTransform viewPort = scroll.transform.Find("Viewport") as RectTransform;

            ScrollRect scrollRect = scroll.GetComponent<ScrollRect>();
            scrollRect.vertical = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;

            RectTransform content = viewPort.transform.Find("Content") as RectTransform;
            HorizontalLayoutGroup layout = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlHeight = layout.childControlWidth = false;
            layout.childForceExpandHeight = layout.childForceExpandWidth = true;
            content.sizeDelta = Vector2.one * 100;
            content.anchorMin = new Vector2(0, 0);
            content.anchorMax = new Vector2(0, 1);

            SimpleScrollView ssv = scroll.AddComponent<SimpleScrollView>();
            ssv.scrollRect = scrollRect;
            ssv.scrollDirection = SimpleScrollView.ScrollDirection.HORIZONTAL;
            ssv.content = content;
            ssv.viewPort = viewPort;
            
            return scroll;
        }
        #endregion

        #region ---------------- 纵向滚动列表 ----------------
        [MenuItem("GameObject/UI/Phenix/VerticalScrollView", false)]
        public static void RegisterVerticalScrollView(MenuCommand menuCommand)
        {
            GameObject scroll = CreateVerticalScrollView(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(scroll, menuCommand);
        }

        static GameObject CreateVerticalScrollView(UIResources resources)
        {
            GameObject scroll = DefaultControls.CreateScrollView(UIDefaultControls.ConvertToDefaultResources(resources));
            scroll.name = "VScrollView";
            Transform toDel1 = scroll.transform.Find("Scrollbar Horizontal");
            Transform toDel2 = scroll.transform.Find("Scrollbar Vertical");
            GameObject.DestroyImmediate(toDel1.gameObject);
            GameObject.DestroyImmediate(toDel2.gameObject);

            RectTransform viewPort = scroll.transform.Find("Viewport") as RectTransform;

            ScrollRect scrollRect = scroll.GetComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;

            RectTransform content = viewPort.transform.Find("Content") as RectTransform;
            VerticalLayoutGroup layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlHeight = layout.childControlWidth = false;
            layout.childForceExpandHeight = layout.childForceExpandWidth = true;
            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = new Vector2(1, 1);

            SimpleScrollView ssv = scroll.AddComponent<SimpleScrollView>();
            ssv.scrollRect = scrollRect;
            ssv.scrollDirection = SimpleScrollView.ScrollDirection.VERTICAL;
            ssv.content = content;
            ssv.viewPort = viewPort;

            return scroll;
        }
        #endregion

        #region ---------------- 弧形列表 ----------------
        [MenuItem("GameObject/UI/Phenix/ArcView", false)]
        public static void RegisterArcView(MenuCommand menuCommand)
        {
            GameObject arcView = CreateArcView(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(arcView, menuCommand);
        }

        static GameObject CreateArcView(UIResources resources)
        {
            GameObject arc = DefaultControls.CreateImage(UIDefaultControls.ConvertToDefaultResources(resources));
            arc.name = "ArcView";
            Image background = arc.GetComponent<Image>();
            background.enabled = false;
            background.raycastTarget = false;                        

            GameObject content = new GameObject("Content", typeof(RectTransform));
            content.transform.parent = arc.transform;            

            GameObject btnSwitch = DefaultControls.CreateButton(UIDefaultControls.ConvertToDefaultResources(resources));
            btnSwitch.name = "BtnSwitch";
            btnSwitch.transform.parent = arc.transform;

            GameObject hotSpot = DefaultControls.CreateImage(UIDefaultControls.ConvertToDefaultResources(resources));
            hotSpot.name = "HotSpot";
            hotSpot.GetComponent<Image>().color = new Vector4(255, 255, 255, 0);
            UIDragable uiDragable = hotSpot.AddComponent<UIDragable>();
            uiDragable.rect = hotSpot.GetComponent<RectTransform>();
            hotSpot.transform.parent = arc.transform;
            hotSpot.AddComponent<PassPointerEvent>();

            ArcView arcView = arc.AddComponent<ArcView>();
            arcView.panel = arc.transform as RectTransform;
            arcView.content = content.transform as RectTransform;
            arcView.hotSpot = uiDragable;
            arcView.btnSwitch = btnSwitch.GetComponent<Button>();            

            return arc;
        }
        #endregion

        #region ---------------- 头顶HUD ----------------
        [MenuItem("GameObject/UI/Phenix/HeadHud", false)]
        public static void RegisterHeadHud(MenuCommand menuCommand)
        {
            GameObject headHud = CreateHeadHud(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(headHud, menuCommand);            
        }

        static GameObject CreateHeadHud(UIResources resources)
        {
            GameObject headHud = new GameObject("HeadHud");
            Canvas canvas = headHud.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            VerticalLayoutGroup layout = headHud.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;
            Billboard billboard = headHud.AddComponent<Billboard>();
            billboard.trans = headHud.transform;

            GameObject hp = DefaultControls.CreateSlider(UIDefaultControls.ConvertToDefaultResources(resources));
            hp.name = "HP";
            GameObject.DestroyImmediate(hp.transform.Find("Handle Slide Area").gameObject);
            hp.transform.parent = headHud.transform;

            GameObject mp = DefaultControls.CreateSlider(UIDefaultControls.ConvertToDefaultResources(resources));
            mp.name = "MP";
            GameObject.DestroyImmediate(mp.transform.Find("Handle Slide Area").gameObject);
            mp.transform.parent = headHud.transform;

            GameObject name = DefaultControls.CreateText(UIDefaultControls.ConvertToDefaultResources(resources));
            name.name = "Name";            
            name.transform.parent = headHud.transform;
            name.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            return headHud;
        }
        #endregion

        #region ---------------- 电视秀 ----------------
        [MenuItem("GameObject/UI/Phenix/TVShow", false)]
        public static void RegisterTVShow(MenuCommand menuCommand)
        {
            GameObject tv = CreateTVShow(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(tv, menuCommand);
            // 以下必须在此执行，若放在CreateTVShow中会因为新建对象层级未绑定以至canvas为null      
            TVShow tvShow = tv.GetComponent<TVShow>();
            tvShow.canvas = tv.GetComponentInParent<Canvas>();
        }

        static GameObject CreateTVShow(UIResources resources)
        {
            GameObject tv = new GameObject("TVShow");
            TVShow tvShow = tv.AddComponent<TVShow>();
            RawImage tvScreen = tv.AddComponent<RawImage>();

            GameObject tvCamera = new GameObject("TVCamera", typeof(UnityEngine.Camera));
            tvCamera.transform.parent = tv.transform;
            UnityEngine.Camera cam = tvCamera.GetComponent<UnityEngine.Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0, 0, 0, 0);

            tvShow.tvCamera = cam;
            tvShow.tvScreen = tvScreen;

            return tv;
        }
        #endregion

        #region ---------------- 多页签(横向) ----------------
        [MenuItem("GameObject/UI/Phenix/HorizontalTabView", false)]
        public static void RegisterHorizontalTabView(MenuCommand menuCommand)
        {
            GameObject tabViewObj = CreateHorizontalTabView(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(tabViewObj, menuCommand);            
        }

        static GameObject CreateHorizontalTabView(UIResources resources)
        {
            GameObject tabViewObj = new GameObject("TabView", typeof(Image));
            TabView tabView = tabViewObj.AddComponent<TabView>();

            GameObject contents = new GameObject("Contents");
            HorizontalLayoutGroup layout = contents.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlHeight = layout.childControlWidth = false;
            layout.childForceExpandHeight = layout.childForceExpandWidth = false;
            ContentSizeFitter csf = contents.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            csf.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            tabView.contents = contents.transform as RectTransform;
            contents.transform.parent = tabViewObj.transform;

            return tabViewObj;
        }
        #endregion

        #region ---------------- 多页签(纵向) ----------------
        [MenuItem("GameObject/UI/Phenix/VerticalTabView", false)]
        public static void RegisterVerticalTabView(MenuCommand menuCommand)
        {
            GameObject tabViewObj = CreateVerticalTabView(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(tabViewObj, menuCommand);
        }

        static GameObject CreateVerticalTabView(UIResources resources)
        {
            GameObject tabViewObj = new GameObject("TabView", typeof(Image));
            TabView tabView = tabViewObj.AddComponent<TabView>();

            GameObject contents = new GameObject("Contents");
            VerticalLayoutGroup layout = contents.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlHeight = layout.childControlWidth = false;
            layout.childForceExpandHeight = layout.childForceExpandWidth = false;
            ContentSizeFitter csf = contents.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            tabView.contents = contents.transform as RectTransform;
            contents.transform.parent = tabViewObj.transform;

            return tabViewObj;
        }
        #endregion

        #region ---------------- 开关 ----------------
        [MenuItem("GameObject/UI/Phenix/Switch", false)]
        public static void RegisterSwitch(MenuCommand menuCommand)
        {
            GameObject toggleOffObj = CreateSwitch(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(toggleOffObj, menuCommand);
        }

        static GameObject CreateSwitch(UIResources resources)
        {
            GameObject switchOff = new GameObject("Switch Off");
            Toggle toggle = switchOff.AddComponent<Toggle>();
            Image targetGraphic = switchOff.AddComponent<Image>();
            GameObject switchOn = new GameObject("Switch On");
            Image graphic = switchOn.AddComponent<Image>();
            switchOn.transform.parent = switchOff.transform;

            toggle.targetGraphic = targetGraphic;
            toggle.graphic = graphic;

            (switchOn.transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
            (switchOn.transform as RectTransform).anchorMin = Vector2.zero;
            (switchOn.transform as RectTransform).anchorMax = Vector2.one;
            (switchOn.transform as RectTransform).offsetMin = Vector2.zero;
            (switchOn.transform as RectTransform).offsetMax = Vector2.one;

            return switchOff;
        }
        #endregion

        #region ---------------- 网格 ----------------
        [MenuItem("GameObject/UI/Phenix/GridView", false)]
        public static void RegisterGridView(MenuCommand menuCommand)
        {
            GameObject gridViewObj = CreateGridView(UIMenuTools.GetStandardResources());
            UIMenuTools.PlaceUIElementRoot(gridViewObj, menuCommand);
        }

        static GameObject CreateGridView(UIResources resources)
        {
            GameObject gridViewObj = DefaultControls.CreateScrollView(UIDefaultControls.ConvertToDefaultResources(resources));
            gridViewObj.name = "GridView";
            GridView gridView = gridViewObj.AddComponent<GridView>();            
            Transform content = gridView.transform.Find("Viewport").Find("Content");
            gridView.content = content as RectTransform;
            content.gameObject.AddComponent<GridLayoutGroup>();
            ScrollRect scrollRect = gridViewObj.GetComponentInChildren<ScrollRect>();
            scrollRect.movementType = ScrollRect.MovementType.Clamped;

            return gridViewObj;
        }
        #endregion
    }

}