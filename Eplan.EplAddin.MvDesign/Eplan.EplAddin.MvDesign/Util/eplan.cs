using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

using Eplan.EplApi.Base;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.DataModel.Topology;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;

namespace Eplan.EplAddin.MvDesign.Util
{
	public class eplan
	{

		public static MDPartsDatabase GetPartMaster()
		{
			MDPartsManagement pm = new MDPartsManagement();
			return pm.OpenDatabase();
		}

		public static Text WriteText(Page page, string contents, double x, double y, TextBase.JustificationType justification = TextBase.JustificationType.BaseLineLeft)
		{
			if (page == null || contents == null) return null;

			Text text = new Text();
			text.Create(page, contents, -16002);
			text.Location = new PointD(x, y);
			text.Justification = justification;

			return text;
		}

		public static Project FindOpenedProject(string name)
		{
			ProjectManager pm = new ProjectManager();
			using (new LockingStep())
			{
				foreach (Project p in pm.OpenProjects)
				{
					if (p.ProjectName == name)
					{
						return p;
					}
				}
			}

			return null;
		}

		public static Project OpenProject(string name)
		{
			ProjectManager pm = new ProjectManager();

			string filePath = pm.Paths.Projects + "\\" + name;
			if (!File.Exists(filePath)) filePath += ".elk";
			if (File.Exists(filePath))
			{
				try
				{
					using (new LockingStep())
					{
						return pm.OpenProject(filePath);
					}
				}
				catch 
				{
					//File.Delete(filePath);
					return null;
				}
			}
			else
				return null;
		}

		public static Project GetEplanProject(string fileName)
		{
			Project p = eplan.FindOpenedProject(fileName);
			if (p != null) return p;

			return OpenProject(fileName);
		}

		public static Project CreateProject(string templatePath, string projectLinkFilePath)
		{
			ProjectManager pm = new ProjectManager();
			if (!File.Exists(templatePath))
			{
				MessageBox.Show("프로젝트 템플릿을 찾을 수 없습니다: " + templatePath);
				return null;
			}

			using (new LockingStep())
			{
				return pm.CreateProject(projectLinkFilePath, templatePath);
			}
		}

		public static WindowMacro InsertWindowMacro(Page page, string macroPath, double x, double y, int nVariant = 0)
		{
			if (page == null) return null;

			if (!File.Exists(macroPath))
			{
				MessageBox.Show("File not found: " + macroPath);
				return null;
			}

			try
			{
				WindowMacro wm = new WindowMacro();
				wm.Open(macroPath, page.Project);

				StorableObject[] objects = new Insert().WindowMacro(
					wm,
					WindowMacro.Enums.RepresentationType.Graphics,
					nVariant,
					page,
					new PointD(x, y),
					Insert.MoveKind.Absolute,
					WindowMacro.Enums.NumerationMode.Number);

				return wm;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
		}


		public static Page GetPageWithProperty(Project project, string name)
		{
			if (project == null) return null;

			name = name.ToUpper();
			foreach (Page p in project.Pages)
			{
				if (p.Properties.PAGE_CUSTOM_SUPPLEMENTARYFIELD87.IsEmpty ||
					p.Properties.PAGE_CUSTOM_SUPPLEMENTARYFIELD88.IsEmpty) continue;

				MultiLangString mv_design = null;
				try
				{
					mv_design = p.Properties.PAGE_CUSTOM_SUPPLEMENTARYFIELD87;

					if (mv_design == null) continue;
					string value = mv_design.GetString(ISOCode.Language.L___).ToUpper();
					if ("MV_DESIGN" != value) continue;

					MultiLangString mls = p.Properties.PAGE_CUSTOM_SUPPLEMENTARYFIELD88;
					string name1 = mls.GetString(ISOCode.Language.L___);
					if (name1.ToUpper() == name)
					{
						return p;
					}
				}
				catch
				{
					continue;
				}
			}

			return null;
		}

		public static Page GetPageWithIdentifyingName(Project project, string name)
		{
			if (project == null) return null;

			name = name.ToUpper();
			foreach (Page p in project.Pages)
			{
				if (p.Name.ToUpper() == name)
				{
					return p;
				}
			}

			return null;
		}

		public static Line DrawLine(Page page, PointD point1, PointD point2)
		{
			if (page == null) return null;

			Line line = new Line();
			line.Create(page);
			line.StartPoint = point1;
			line.EndPoint = point2;

			return line;
		}

		public static Rectangle DrawRectangle(Page page, PointD point1, PointD point2)
		{
			if (page == null) return null;

			Rectangle r = new Rectangle();
			r.Create(page);
			r.Location = point1;
			r.SetArea(point1, point2);

			return r;
		}

		public static int GetLastPageNo(Project project)
		{
			if (project == null) return 0;

			int lastIndex = 0;
			int index = 0;
			foreach (Page p in project.Pages)
			{
				try
				{
					index = Convert.ToInt32(p.Properties.PAGE_COUNTER.ToString());
				}
				catch
				{
					MessageBox.Show("ERROR: " + p.Properties.PAGE_COUNTER.ToString());
				}
				
				if (index > lastIndex)
				{
					lastIndex = index;
				}
			}

			return lastIndex;
		}

		public static Page AddPageMacro(Project project, string macroPath, string facility, string plant, string location, string doc_type, string nomination, DocumentTypeManager.DocumentType type)
		{
			if (!File.Exists(macroPath))
			{
				MessageBox.Show("File not found: " + macroPath);
				return null;
			}

			string name = string.Empty;
			if (facility != null && facility.Length > 0)
				name += "==" + facility;

			if (plant != null && plant.Length > 0)
				name += "=" + plant;

			if (location != null && location.Length > 0)
				name += "+" + location;

			if (doc_type != null && doc_type.Length > 0)
				name += "&" + doc_type;

			name += "/" + string.Format("{0:D}", Util.eplan.GetLastPageNo(project) + 1);

			PageMacro pageMacro = new PageMacro();
			pageMacro.Open(macroPath, project);

			bool[] overwirtes = new bool[pageMacro.Pages.Length];
			for (int i=0; i<overwirtes.Length; i++) 
			{
				overwirtes[i] = true;
			}

			pageMacro.Pages[0].Name = name;

			MultiLangString mls = new MultiLangString();
			mls.AddString(ISOCode.Language.L___, nomination);
			pageMacro.Pages[0].Properties.PAGE_NOMINATIOMN = mls;

			StorableObject[] sos = new Insert().PageMacro(pageMacro, project, overwirtes, PageMacro.Enums.NumerationMode.None);
			if (sos.Length > 0)
			{
				Page page = null;
				for (int i = 0; i < sos.Length; i++)
				{
					page = sos[i] as Page;
					page.PageType = type;
				}
				return page;
			}
			else
				return null;
		}

		public static Page AddPageMacro(Project project, string facility, string plant, string location, string doc_type, string nomination, DocumentTypeManager.DocumentType type)
		{
			string macroPath = Data.Location.Config + @"\hhi_default.emp";
			return AddPageMacro(project, macroPath, facility, plant, location, doc_type, nomination, type);
		}

		public static Page AddPage(Project project, string facility, string plant, string location, string doc_type, string nomination, DocumentTypeManager.DocumentType type)
		{
			PagePropertyList oPagePropList = new PagePropertyList();
			if (facility != null)
			{
				oPagePropList[Eplan.EplApi.DataModel.Properties.Page.DESIGNATION_FUNCTIONALASSIGNMENT] = facility;
			}

			if (plant != null)
			{
				oPagePropList[Eplan.EplApi.DataModel.Properties.Page.DESIGNATION_PLANT] = plant;
			}

			if (location != null)
			{
				oPagePropList[Eplan.EplApi.DataModel.Properties.Page.DESIGNATION_LOCATION] = location;
			}

			if (doc_type != null)
			{
				oPagePropList[Eplan.EplApi.DataModel.Properties.Page.DESIGNATION_DOCTYPE] = doc_type;
			}

			oPagePropList[Eplan.EplApi.DataModel.Properties.Page.PAGE_COUNTER] = GetLastPageNo(project) + 1;
			oPagePropList[Eplan.EplApi.DataModel.Properties.Page.PAGE_CUSTOM_SUPPLEMENTARYFIELD88] = "MV_DESIGN";
			oPagePropList[Eplan.EplApi.DataModel.Properties.Page.PAGE_CUSTOM_SUPPLEMENTARYFIELD88] = type.ToString();

			Page page = new Page();
			page.Create(project, type, oPagePropList);

			if (page != null && nomination != null)
			{
				MultiLangString mls = new MultiLangString();
				mls.AddString(ISOCode.Language.L___, nomination);
				page.Properties.PAGE_NOMINATIOMN = nomination;
			}

			return page;
		}

		public static Page AddShipPanellayoutPage(Project project, string macroPath, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, macroPath, null, plant, null, doc_type, nomination, DocumentTypeManager.DocumentType.PanelLayout);
			
			return page;
		}

		public static Page AddShipGraphicPage(Project project, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, null, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.Graphics);
			if (page != null)
			{
				page.Properties.PAGE_SCALE = 22.0;
			}

			return page;
		}

		public static Page AddLandGraphicPage(Project project, string facility, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, facility, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.Graphics);
			
			if (page != null)
			{
				page.Properties.PAGE_SCALE = 25.0;
			}

			return page;
		}

		public static Page AddLandLayoutPage(Project project, string facility, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, facility, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.PanelLayout);

			if (page != null)
			{
				page.Properties.PAGE_SCALE = 25.0;
			}

			return page;
		}

		public static Page AddShipSinglelinePage(Project project, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, null, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.CircuitSingleLine);
			
			if (page != null)
			{
				page.Properties.PAGE_SCALE = 1.0;
			}

			return page;
		}

		public static Page AddLandSinglelinePage(Project project, string facility, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, facility, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.CircuitSingleLine);
			
			if (page != null)
			{
				page.Properties.PAGE_SCALE = 1.0;
			}

			return page;
		}

		public static Page AddShipThreelinePage(Project project, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, null, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.Circuit);
			if (page != null)
			{
				page.Properties.PAGE_SCALE = 1.0;
			}

			return page;
		}

		public static Page AddLandThreelinePage(Project project, string facility, string plant, string location, string doc_type, string nomination)
		{
			Page page = AddPageMacro(project, facility, plant, location, doc_type, nomination, DocumentTypeManager.DocumentType.Circuit);
			
			if (page != null)
			{
				page.Properties.PAGE_SCALE = 1.0;
			}

			return page;
		}



		public static StorableObject[] AddGraphicMacro(Page page, string macroPath, int nVariant, double x, double y)
		{
			if (page == null) return null;

			// eplan.AddText(page, null, macroPath, x, y);
			Insert insert = new Insert();
			WindowMacro windowMacro = new WindowMacro();

			if (!File.Exists(macroPath))
			{
				//MessageBox.Show("File not found: " + macroPath);
				return null;
			}

			windowMacro.Open(macroPath, page.Project);
			windowMacro.Variant = nVariant;
			try
			{
				StorableObject[] objects = insert.WindowMacro(windowMacro,
															  WindowMacro.Enums.RepresentationType.Graphics,
															  nVariant,
															  page,
															  new PointD(x, y),
															  Insert.MoveKind.Absolute,
															  WindowMacro.Enums.NumerationMode.NumberPreferPrefix);
				new NameService().EvaluateAndSetAllNames(page);
				return objects;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
		}

		public static StorableObject[] AddSinglelineMacro(Page page, string macroPath, int nVariant, double x, double y)
		{
			if (page == null) return null;

			// eplan.AddText(page, null, macroPath, x, y);
			Insert insert = new Insert();
			WindowMacro windowMacro = new WindowMacro();

			if (!File.Exists(macroPath))
			{
				//MessageBox.Show("File not found: " + macroPath);
				return null;
			}

			windowMacro.Open(macroPath, page.Project);
			windowMacro.Variant = nVariant;
			try
			{
				StorableObject[] objects = insert.WindowMacro(windowMacro,
															  WindowMacro.Enums.RepresentationType.SingleLine,
															  nVariant,
															  page,
															  new PointD(x, y),
															  Insert.MoveKind.Absolute,
															  WindowMacro.Enums.NumerationMode.NumberPreferPrefix);
				new NameService().EvaluateAndSetAllNames(page);
				return objects;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
		}

		public static StorableObject[] AddThreelineMacro(Page page, string macroPath, int nVariant, double x, double y)
		{
			if (page == null) return null;

			// eplan.AddText(page, null, macroPath, x, y);
			Insert insert = new Insert();
			WindowMacro windowMacro = new WindowMacro();

			if (!File.Exists(macroPath))
			{
				//MessageBox.Show("File not found: " + macroPath);
				return null;
			}

			windowMacro.Open(macroPath, page.Project);
			windowMacro.Variant = nVariant;
			try
			{
				StorableObject[] objects = insert.WindowMacro(windowMacro,
															  WindowMacro.Enums.RepresentationType.MultiLine,
															  nVariant,
															  page,
															  new PointD(x, y),
															  Insert.MoveKind.Absolute,
															  WindowMacro.Enums.NumerationMode.NumberPreferPrefix);
				new NameService().EvaluateAndSetAllNames(page);
				return objects;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
		}

		public static LocationBox GetSingleline(StorableObject[] sos)
		{
			foreach (StorableObject so in sos)
			{
				LocationBox lb = so as LocationBox;
				if (lb == null) continue;

				System.Drawing.RectangleF r = eplan.GetApproxBoundingBox(lb);
				if (r.Width > 100) return lb;
			}

			return null;
		}
		
		public static LocationBox AddSinglelineMacro(bool isShip, Page page, string macroPath, Data.MvFeeder feeder, double x, double y)
		{
			if (page == null) return null;
			if (!File.Exists(macroPath)) return null;
			
			WindowMacro windowMacro = new WindowMacro();
			windowMacro.Open(macroPath, page.Project);
			StorableObject[] objects = new Insert().WindowMacro(windowMacro,
														  WindowMacro.Enums.RepresentationType.SingleLine,
														  0,
														  page,
														  new PointD(x, y),
														  Insert.MoveKind.Absolute,
														  WindowMacro.Enums.NumerationMode.None);
			SetLocationBoxName(isShip, feeder, objects);
			UnsetFunctionIsMainFunction(objects);

			SetLoadData(isShip, feeder, objects);
			
			new NameService().EvaluateAndSetAllNames(page);

			return GetSingleline(objects);
		}



		public static Page AddThreelineMacro(bool isShip, Page page, string macroPath, Data.MvFeeder feeder, double x, double y)
		{
			if (page == null) return null;
			if (!File.Exists(macroPath)) return null;

			WindowMacro windowMacro = new WindowMacro();
			windowMacro.Open(macroPath, page.Project);
			StorableObject[] objects = new Insert().WindowMacro(windowMacro,
														  WindowMacro.Enums.RepresentationType.MultiLine,
														  0,
														  page,
														  new PointD(x, y),
														  Insert.MoveKind.Absolute,
														  WindowMacro.Enums.NumerationMode.None);
			UnsetFunctionIsMainFunction(objects);

			new NameService().EvaluateAndSetAllNames(page);
			return page;
		}


		private static void SetLocationBoxName(bool isShip, Data.MvFeeder feeder, StorableObject[] sos)
		{
			LocationBox locationBox = null;
			double width = 0;
			for (int i = 0; i < sos.Length; i++)
			{
				LocationBox lb = sos[i] as LocationBox;
				if (lb == null) continue;

				PointD[] points = lb.GetBoundingBox();

				if (Math.Abs(points[1].X - points[0].X) > width)
				{
					width = Math.Abs(points[1].X - points[0].X);
					locationBox = lb;
				}
			}

			SetSinglelineName(isShip, feeder, locationBox);
			return;
		}

		private static string GetString(string s)
		{
			if (s.Length < 1) return "-";
			else return s;
		}

		private static void SetLoadData(bool isShip, Data.MvFeeder feeder, StorableObject[] sos)
		{
			for (int i = 0; i < sos.Length; i++)
			{
				BoxedDevice bd = sos[i] as BoxedDevice;
				if (bd == null || !bd.VisibleName.ToUpper().Contains("LOAD")) continue;

				bd.IsMainFunction = true;

				if (isShip)
				{
					string cableSize = GetString(feeder.Column01);
					string lugSize = "-";
					int lugQty = 0;

					int length = "H6KT".Length;
					if (cableSize != null && cableSize.Length >= "H6KT".Length)
					{
						int index = cableSize.IndexOfAny(new char[] { '*' });
						if (index >= length)
						{
							lugSize = cableSize.Substring(length, cableSize.IndexOf('*') - length) + "-12";
							int.TryParse(cableSize.Substring(cableSize.LastIndexOf("*") + 1), out lugQty);
							lugQty *= 3;
						}
						else
						{
							lugQty = 0;
						}
					}

					
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[1] = GetString(feeder.FeederNo);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[14] = GetString(feeder.ConsumerKW);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[5] = GetString(feeder.ConsumerA);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[6] = cableSize;// Cable Size = CableSq Yard
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[11] = GetString(feeder.Column02);
					//bd.Properties.FUNC_SUPPLEMENTARYFIELD[7] = GetString(feeder.Column03);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[3] = GetString(feeder.PanelName1) + "\n" + GetString(feeder.PanelName2);

					bd.Properties.FUNC_SUPPLEMENTARYFIELD[4] = GetString(feeder.Column04);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[7] = lugSize;
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[8] = lugQty.ToString();
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[9] = GetString(feeder.Column05);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[10] = GetString(feeder.Column06);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[12] = GetString(feeder.Column07);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[13] = GetString(feeder.Column08);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[20] = GetString(feeder.PanelName1) + "\n" + GetString(feeder.PanelName2);
				}
				else
				{
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[1] = GetString(feeder.FeederNo);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[2] = GetString(feeder.ConsumerKW);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[3] = GetString(feeder.ConsumerA);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[4] = GetString(feeder.Column01);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[5] = GetString(feeder.Column02);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[6] = GetString(feeder.Column03);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[7] = GetString(feeder.Column04);
					bd.Properties.FUNC_SUPPLEMENTARYFIELD[8] = GetString(feeder.PanelName1) + "\n" + GetString(feeder.PanelName2) + "\n" + GetString(feeder.PanelName3);

				}
			}
		}

		private static void SetSinglelineName(bool isShip, Data.MvFeeder feeder, LocationBox lb)
		{
			if (lb == null) return;
			if (isShip)
			{
				lb.Name = lb.VisibleName = "=VOL.5+" + feeder.FeederNo;
			}
			else
			{
				lb.Name = lb.VisibleName = "=" + feeder.FeederNo;
			}
		}

		private static void UnsetFunctionIsMainFunction(StorableObject[] objects)
		{
			foreach (StorableObject so in objects)
			{
				Function function = so as Function;
				if (function != null)
				{
					if (function.IsNameEmpty) continue;
					try
					{
						function.IsMainFunction = true;
						foreach (ArticleReference ar in function.ArticleReferences)
						{
							function.RemoveArticleReference(ar);
							ar.StoreToObject();
						}

						function.IsMainFunction = false;
						function.Properties.FUNC_TECHNICAL_CHARACTERISTIC = string.Empty;
					}
					catch (Exception ex)
					{
						string message = string.Format("Funtion Name: {0}\n Page Name: {1}\n Location: ({2}, {3})\n", function.Name, function.Page.Name, function.Location.X, function.Location.Y);
						MessageBox.Show(message + "\n" + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
		}


		public static double GetWidth(Project project, string macroPath)
		{
			double width = 0.0;
			try
			{
				WindowMacro wm = new WindowMacro();
				wm.Open(macroPath, project);
				PointD[] sizes = wm.GetBoundingBox();
				width = Math.Abs(sizes[1].X - sizes[0].X);

				width = ((int)width / 10) * 10;
			}
			catch
			{
				MessageBox.Show("ERROR: Failed to open Macro: " + macroPath);
			}

			return width;
		}

		public static Text AddText(Page page, string layerName, string value, double x, double y)
		{
			if (page == null) return null;

			Text text = new Text();
			MultiLangString mls = new MultiLangString();
			mls.AddString(ISOCode.Language.L_ko_KR, value);
			mls.AddString(ISOCode.Language.L_en_US, value);

			GraphicalLayer layer = page.Project.LayerTable["EPLAN108"];
			if (layer == null)
			{
				text.Layer = page.Project.LayerTable.Layers[0];
			}

			text.Create(page, mls, layer.TextHeight);
			text.Height = -16002;
			text.Location = new PointD(x, y);
			text.Justification = TextBase.JustificationType.MiddleCenter;

			return text;
		}

		public static DimensionItem AddDimension(Page page, string layerName, double value, PointD point1, PointD point2, double height)
		{
			if (page == null) return null;

			PointD point3 = new PointD((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2 + height);
			DimensionItem item = new DimensionItem();
			//item.Create(page, point1, point2, point3, DimensionItem.Enums.DimensionType.Linear);
			item.Create(page, point1, point2, point3);

			item.DimensionShowUnits = false;
			GraphicalLayer layer = page.Project.LayerTable["EPLAN107"];
			if (layer == null)
			{
				layer = page.Project.LayerTable.Layers[0];
			}

			item.Layer = layer;

			return item;
		}

		public static DimensionItem AddDimension(Page page, string layerName, double value, double x1, double y1, double x2, double y2, double height)
		{
			PointD point1 = new PointD(x1, y1);
			PointD point2 = new PointD(x2, y2);
			return AddDimension(page, layerName, value, point1, point2, height);
		}

		public static LocationBox FindSingleLine(Project project, string groupNo, string feederNo)
		{
			if (project == null) return null;

			groupNo = groupNo.ToUpper();
			feederNo = feederNo.ToUpper();

			foreach (Page page in project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.CircuitSingleLine) continue;
				if (groupNo != null && groupNo.Length > 0)
				{
					if (!page.IdentifyingName.StartsWith(groupNo)) continue;
				}

				foreach (Placement p in page.AllFirstLevelPlacements)
				{
					LocationBox lb = p as LocationBox;
					if (lb == null) continue;
					if (lb.Name.ToUpper() != feederNo) continue;

					return lb;
				}
			}

			return null;
		}



		public static LocationBox FindShipSingleLine(Project project, string feederNo)
		{
			if (project == null) return null;

			feederNo = feederNo.ToUpper();

			foreach (Page page in project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.CircuitSingleLine) continue;
				if (!page.IdentifyingName.StartsWith("=VOL.4")) continue;

				foreach (Placement p in page.AllFirstLevelPlacements)
				{
					LocationBox lb = p as LocationBox;
					if (lb == null) continue;
					if (lb.Name.ToUpper() != feederNo) continue;

					return lb;
				}
			}

			return null;
		}

		public static LocationBox FindLandSingleLine(Project project, string groupNo, string feederNo)
		{
			if (project == null) return null;

			groupNo = groupNo.ToUpper();
			feederNo = feederNo.ToUpper();

			foreach (Page page in project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.CircuitSingleLine) continue;
				if (groupNo != null && groupNo.Length > 0)
				{
					if (!page.IdentifyingName.StartsWith(groupNo)) continue;
				}

				foreach (Placement p in page.AllFirstLevelPlacements)
				{
					LocationBox lb = p as LocationBox;
					if (lb == null) continue;
					if (lb.Name.ToUpper() != feederNo) continue;

					return lb;
				}
			}

			return null;
		}

		public static System.Drawing.RectangleF GetBoundingBox(Placement placement)
		{
			if (placement == null) return new System.Drawing.RectangleF();

			PointD[] points = placement.GetBoundingBox();



			System.Drawing.RectangleF r = new System.Drawing.RectangleF();
			r.X = points[0].X < points[1].X ? (float)points[0].X : (float)points[1].X;
			r.Y = points[0].Y < points[1].Y ? (float)points[0].Y : (float)points[1].Y;
			r.Width = (float)(points[1].X - points[0].X);
			r.Height = (float)(points[1].Y - points[0].Y);

			string message = string.Format("({0}, {1}), ({2}, {3}) W={4}, H={5}", points[0].X, points[0].Y, points[1].X, points[1].Y, r.Width, r.Height);
			//MessageBox.Show(message);

			return r;
		}


		public static System.Drawing.RectangleF GetApproxBoundingBox(Placement placement)
		{
			if (placement == null) return new System.Drawing.RectangleF();

			PointD[] points = placement.GetBoundingBox();
			int delta = 1;
			LocationBox lb = placement as LocationBox;
			if (lb != null)
				delta = 0;
				

			System.Drawing.RectangleF r = new System.Drawing.RectangleF();
			if (points[0].X < points[1].X)
			{
				r.X = (float) points[0].X - delta;
				r.Width = (float)(points[1].X - points[0].X) + 2*delta;
			}
			else
			{
				r.X = (float)points[1].X - delta;
				r.Width = (float)(points[0].X - points[1].X) + 2 * delta;
			}

			if (points[0].Y < points[1].Y)
			{
				r.Y = (float)points[0].Y - delta;
				r.Height = (float)(points[1].Y - points[0].Y) + 2 * delta;
			}
			else
			{
				r.Y = (float)points[1].Y - delta;
				r.Height = (float)(points[0].Y - points[1].Y) + 2 * delta;
			}

			return r;
		}

		private static System.Drawing.PointF GetLocation(Placement placement)
		{
			Line line = placement as Line;
			if (line != null)
			{
				double dx = line.StartPoint.X - line.EndPoint.X;
				double dy = line.StartPoint.Y - line.EndPoint.Y;
				double length = Math.Sqrt(dx * dx + dy * dy);
				if (length < 120)
				{
					float x = (float)((line.StartPoint.X + line.EndPoint.X) / 2);
					float y = (float)((line.StartPoint.Y + line.EndPoint.Y) / 2);

					return new System.Drawing.PointF(x, y);
				}
			}

			return new System.Drawing.PointF((float)placement.Location.X, (float)placement.Location.Y);
		}

		public static WindowMacro CreateWindowMacro(LocationBox locationBox)
		{
			System.Drawing.RectangleF r = GetApproxBoundingBox(locationBox);
			List<Placement> placements = new List<Placement>();
			placements.Add(locationBox);
			foreach (Placement placement in locationBox.Page.AllPlacements)
			{
				BoxedDevice bd = placement as BoxedDevice;
				if (bd != null && bd.VisibleName.ToUpper().Replace("-", string.Empty).StartsWith("LOAD")) continue;

				System.Drawing.PointF point = GetLocation(placement);
				if (r.Contains(point))
				{
					placements.Add(placement);
				}
			}
			
			WindowMacro wm = new WindowMacro();
			Placement[] p_array = placements.ToArray();
			string macroPath = locationBox.Project.ProjectDirectoryPath + "\\MV_DESIGN\\singleline.ema";
			wm.Create(macroPath, 0, p_array, null);

			return wm;
		}


		public static WindowMacro CopySingleline(LocationBox locationBox)
		{
			System.Drawing.RectangleF r = GetApproxBoundingBox(locationBox);
			List<Placement> placements = new List<Placement>();

			foreach (Placement placement in locationBox.Page.AllPlacements)
			{
				BoxedDevice bd = placement as BoxedDevice;
				if (bd != null && bd.VisibleName.ToUpper().Equals("LOAD")) continue;

				System.Drawing.PointF point = GetLocation(placement);
				if (r.Contains(point))
				{
					//placement.Location = new PointD(placement.Location.X - locationBox.Location.X, placement.Location.Y - locationBox.Location.Y);
					placements.Add(placement);
				}
			}

			WindowMacro wm = new WindowMacro();
			Placement[] p_array = placements.ToArray();
			string macroPath = locationBox.Project.ProjectDirectoryPath + "\\MV_DESIGN\\singleline_copied.ema";
			wm.Create(macroPath, 0, p_array, null);

			return wm;
		}


		public static LocationBox DeleteSingleline(LocationBox locationBox, bool skipLoad = true)
		{
			if (locationBox == null) return null;

			System.Drawing.RectangleF r = GetApproxBoundingBox(locationBox);
			foreach (Placement p in locationBox.Page.AllPlacements)
			{		
				if (p != locationBox)
				{
					System.Drawing.Point location = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
					if (r.Contains(location))
					{
						if (skipLoad)
						{
							BoxedDevice bd = p as BoxedDevice;
							if (bd != null && bd.VisibleName.ToUpper().Equals("LOAD")) continue;
						}

						p.Remove();
					}
				}
			}

			return locationBox;
		}

		public static Page DeleteThreeline(Page page)
		{
			foreach (Placement p in page.AllPlacements)
			{
				p.Remove();
			}

			return page;
		}

		public static LocationBox PasteSingleline(LocationBox from, LocationBox to)
		{
			System.Drawing.RectangleF r = GetApproxBoundingBox(from);
			List<Placement> list = new List<Placement>();
			foreach (Placement p in from.Page.AllPlacements)
			{
				BoxedDevice bd = p as BoxedDevice;
				if (bd != null && bd.VisibleName.ToUpper().Equals("LOAD")) continue;

				if (p != from)
				{
					System.Drawing.Point location = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
					if (r.Contains(location))
					{
					    Placement placement = p.CopyTo(to.Page);
						placement.Location = new PointD(p.Location.X - from.Location.X + to.Location.X, p.Location.Y - from.Location.Y + to.Location.Y);
					}
				}
			}

			new NameService().EvaluateAndSetAllNames(to.Page);

			return to;
		}



		public static Page PasteThreeline(string fromName, Page from, string toName, Page to)
		{
			foreach (Placement p in from.AllPlacements)
			{
				Placement placement = p.CopyTo(to);

				Function funcion = placement as Function;
				if (funcion != null)
				{
					funcion.Name = funcion.Name.ToUpper().Replace(fromName, toName);
				}
			}

			return to;
		}

		public static WindowMacro InsertWindowMacro(Page page, WindowMacro wm, double x, double y, int nVariant = 0)
		{
			if (page == null) return null;

			try
			{
				StorableObject[] objects = new Insert().WindowMacro(
					wm,
					WindowMacro.Enums.RepresentationType.Graphics,
					nVariant,
					page,
					new PointD(x, y),
					Insert.MoveKind.Absolute,
					WindowMacro.Enums.NumerationMode.Number);

				return wm;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
		}

		public static LocationBox RemoveLandSingleLine(Project project, string groupNo, string feederNo)
		{
			LocationBox locationBox = FindSingleLine(project, groupNo, feederNo);
			if (locationBox == null) return null;

			System.Drawing.RectangleF r = GetApproxBoundingBox(locationBox);

			foreach (Placement p in locationBox.Page.AllPlacements)
			{
				if (p.Equals(locationBox)) continue;

				System.Drawing.Point point = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
				if (r.Contains(point))
				{
					p.Remove();
				}
			}

			return locationBox;
		}

		public static Page FindThreeline(Project project, string threelineName)
		{
			if (project == null) return null;

			threelineName = threelineName.ToUpper();

			foreach (Page p in project.Pages)
			{
				if (p.IdentifyingName.ToUpper().StartsWith(threelineName))
					return p;

			}

			return null;
		}

		public static Page FindThreeline(Project project, string groupNo, string threelineName)
		{
			if (project == null) return null;

			threelineName = threelineName.ToUpper();

			foreach (Page p in project.Pages)
			{
				if (p.IdentifyingName.ToUpper().StartsWith(threelineName))
					return p;

			}

			return null;
		}




		public static Function RemoveArticleReferences(Function function)
		{
			if (function == null) return null;
			function.IsMainFunction = true;
			foreach (ArticleReference ar in function.ArticleReferences)
			{
				function.RemoveArticleReference(ar);
			}

			function.Properties.FUNC_TECHNICAL_CHARACTERISTIC = string.Empty;
			function.IsMainFunction = false;

			return function;
		}


		public static Function RemoveARFromThreeline(Page page, string name)
		{
			name = name.ToUpper();
			foreach (Function f in page.Functions)
			{
				BoxedDevice boxedDevice = f as BoxedDevice;
				if (boxedDevice == null) continue;

				if (boxedDevice.Name.ToUpper() == name)
				{
					return RemoveArticleReferences(boxedDevice);
				}
			}

			return null;
		}

		public static Article AddArticle(Project project, string partNr, int variant)
		{
			if (project == null) return null;

			foreach(Article a in project.Articles)
			{
				if (partNr == a.PartNr)
					return a;
			}

			Article article = new Article();
			article.Create(project, partNr, variant.ToString());
			article.LoadFromMasterdata();

			return article;
		}

		

		public static Function AddArticleReference(Function function, string partNr, int variant)
		{
			if (function == null) return null;
	
			try
			{
				RemoveArticleReferences(function);
				function.IsMainFunction = true;
				ArticleReference ar = function.AddArticleReference(partNr);
				ar.StoreToObject();
				new DeviceService().UpdateDevice(function);

				if (ar.Article != null && !ar.Article.Properties.ARTICLE_CHARACTERISTICS.IsEmpty)
				{
					function.Properties.FUNC_TECHNICAL_CHARACTERISTIC = ar.Article.Properties.ARTICLE_CHARACTERISTICS;
				}
				else
				{
					function.Properties.FUNC_TECHNICAL_CHARACTERISTIC = partNr;
				}

				return function;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);

				return null;
			}
		}

		public static Function RemoveARFromPanel(Project project, string name)
		{
			foreach (Page page in project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.PanelLayout) continue;

				foreach (Function f in page.Functions)
				{
					if (f.Name == name)
					{
						Function funtion = RemoveArticleReferences(f);
						return f;
					}
				}
			}

			return null;
		}


		public static BoxedDevice GetBoxedDevice(Page page, System.Drawing.Point point)
		{
			foreach (Function f in page.Functions)
			{
				BoxedDevice bd = f as BoxedDevice;
				if (bd == null) continue;

				System.Drawing.RectangleF r = GetApproxBoundingBox(bd);
				if (r.Contains(point))
				{
					return bd;
				}
			}

			return null;
		}

		public static SymbolReference GetSymbolReference(Page page, System.Drawing.Point point)
		{
			foreach (Placement f in page.AllPlacements)
			{
				SymbolReference sr = f as SymbolReference;
				if (sr == null) continue;

				System.Drawing.RectangleF r = GetApproxBoundingBox(sr);
				if (r.Contains(point))
				{
					return sr;
				}
			}

			return null;
		}

		//public static string GetBoxedDeviceName(Page page, System.Drawing.Point point)
		//{
		//    BoxedDevice bd = GetBoxedDevice(page, point);
		//    if (bd != null)
		//        return bd.Name;
		//    else
		//        return null;
		//}



		private static PointD GetNearestLocation(Page page, System.Drawing.Point point)
		{
			PointD location = new PointD(point.X, point.Y);


			return location;
		}

		private static int GetNumber(ref string name)
		{
			string name1 = name;
			name = Regex.Replace(name, "[0-9]*$", string.Empty).ToUpper();
			int result = 0;
			int.TryParse(name1.Substring(name.Length), out result);

			return result;
		}

		private static string GetNewSymbolName(LocationBox locationBox, string type)
		{
			if (locationBox == null || locationBox.Page == null) return null;
			System.Drawing.RectangleF r = GetApproxBoundingBox(locationBox);

			int lastIndex = 0;
			type = type.ToUpper();
			foreach (Placement pl in locationBox.Page.AllPlacements)
			{
				BoxedDevice bd = pl as BoxedDevice;
				if (bd == null) continue;

				if (!r.Contains(GetLocation(bd))) continue;

				string name = bd.Name.Substring(bd.Name.LastIndexOfAny(new char[] {'-', '+'}) + 1);
				int index = GetNumber(ref name);
				if (type == name && lastIndex < index)
				{
					lastIndex = index;
				}
			}

			lastIndex++;
			return type += (lastIndex).ToString();
		}


		private static Eplan.EplApi.DataModel.Group CreateGroup(StorableObject[] sos)
		{
			List<Placement> list = new List<Placement>();
			foreach (StorableObject so in sos)
			{
				Placement p = so as Placement;
				if (p == null) continue;

				list.Add(p);
			}

			Eplan.EplApi.DataModel.Group group = new EplApi.DataModel.Group();
			group.Create(list.ToArray());

			return group;
		}


		public static BoxedDevice AddSinglelineSymbol(LocationBox locationBox, string type, string partNr, System.Drawing.Point point, int variant)
		{

			MDPartsDatabase mdb = GetPartMaster();
			MDPart mdPart = mdb.GetPart(partNr);
			if (mdPart == null) return null;

			PointD location = GetNearestLocation(locationBox.Page, point);
			string name = GetNewSymbolName(locationBox, type);

			string macroPath = mdPart.Properties.ARTICLE_GROUPSYMBOLMACRO.ToString();
			WindowMacro wm = new WindowMacro();
			wm.Open(macroPath, locationBox.Page.Project);

			StorableObject[] sos = new Insert().WindowMacro(
				wm,
				WindowMacro.Enums.RepresentationType.SingleLine,
				variant,
				locationBox.Page,
				location,
				Insert.MoveKind.Absolute,
				WindowMacro.Enums.NumerationMode.NumberPreferPrefix);

			CreateGroup(sos);

			foreach (StorableObject so in sos)
			{
				BoxedDevice bd = so as BoxedDevice;
				if (bd != null)
				{
					try
					{
						eplan.RemoveArticleReferences(bd);
						string name1 = bd.Name.Substring(0, bd.Name.LastIndexOfAny(new char[] { '-', '+' }) + 1);
						bd.Name = name1 + name;
						return bd;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
						return null;
					}
				}
			}

			return null;
		}



		public static List<Placement> DeleteSinglelineSymbol(Function function)
		{
			BoxedDevice bd = function as BoxedDevice;
			if (bd == null) return null;
			string name = bd.Name;
			System.Drawing.RectangleF r = GetApproxBoundingBox(bd);
			List<Placement> list = new List<Placement>();
			foreach (Placement p in bd.Page.AllPlacements)
			{
				if (p != bd)
				{
					System.Drawing.Point location = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
					if (r.Contains(location))
					{
						list.Add(p);
						p.Remove();
					}
				}
			}

			bd.Remove();
			return list;
		}

	


		public static List<Pin> GetPinsInFunction(Function function)
		{
			if (function == null) return null;

			string name = function.Name;
			System.Drawing.RectangleF r = GetApproxBoundingBox(function);
			List<Pin> list = new List<Pin>();
			foreach (Placement p in function.Page.AllPlacements)
			{
				if (p != function)
				{
					System.Drawing.Point location = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
					if (r.Contains(location))
					{
						Function f = p as Function;
						if (f == null) continue;

						list.AddRange(f.Pins);
					}
				}
			}

			return list;
		}


		private static List<Placement> DeleteThreelineSymbol(BoxedDevice bd)
		{
			List<Placement> list = new List<Placement>();
			System.Drawing.RectangleF r = GetApproxBoundingBox(bd);

			foreach (Placement p in bd.Page.AllPlacements)
			{
				if (p == bd) continue;

				System.Drawing.PointF point = GetLocation(p);
				if (r.Contains(point))
				{
					list.Add(p);
					p.Remove();
				}
			}

			bd.Remove();

			return list;
		}

		public static List<Placement> DeleteThreelineSymbol(Page page, string name)
		{
			foreach (Function f in page.Functions)
			{
				BoxedDevice boxedDevice = f as BoxedDevice;
				if (boxedDevice == null || !boxedDevice.IsPlaced) continue;

				if (boxedDevice.Name == name)
				{
					return DeleteThreelineSymbol(boxedDevice);
				}
			}

			return null;
		}


		public static void RecoverItems(Page page, List<Placement> list)
		{
			foreach (Placement p in list)
			{
				p.CopyTo(page);
			}
		}


		public static PointD GetSymbolLocation(Page page)
		{
			System.Drawing.RectangleF r = new System.Drawing.RectangleF(0, 0, 420, 297);
			double x = 0;
			double width = 0;
			foreach (Placement p in page.AllPlacements)
			{
				BoxedDevice bd = p as BoxedDevice;
				if (bd == null) continue;

				System.Drawing.PointF point = new System.Drawing.PointF((float)bd.Location.X, (float)bd.Location.Y);
				if (r.Contains(point)) continue;

				if (x < bd.Location.X)
				{
					x = bd.Location.X;
					PointD[] points = bd.GetBoundingBox();
					width = Math.Abs(points[0].X - points[1].X);
				}
			}

			return new PointD(x + width + 5, 300);
		}

		private static Function AddSymbol(Page page, string name, string partNr, int variant,WindowMacro.Enums.RepresentationType type)
		{
			MDPartsDatabase mdb = GetPartMaster();
			MDPart mdPart = mdb.GetPart(partNr);
			if (mdPart == null) return null;

			PointD location = GetSymbolLocation(page);

			string macroPath = mdPart.Properties.ARTICLE_GROUPSYMBOLMACRO.ToString();
			WindowMacro wm = new WindowMacro();
			wm.Open(macroPath, page.Project);
			PointD[] points = wm.GetBoundingBox();
			location.Y += Math.Abs(points[0].Y - points[1].Y) / 2;
			location.X += Math.Abs(points[0].X - points[1].X) / 2;
			StorableObject[] sos = new Insert().WindowMacro(
				wm,
				type,
				variant,
				page,
				location,
				Insert.MoveKind.Absolute,
				WindowMacro.Enums.NumerationMode.NumberPreferPrefix);

			foreach (StorableObject so in sos)
			{
				Function bd = so as Function;
				if (bd != null)
				{
					try
					{
						bd.Name = name;
						return bd;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
					}
				}
			}

			return null;
		}

		public static Function AddThreelineSymbol(Page page, string name, string partNr, int variant)
		{
			MDPartsDatabase mdb = GetPartMaster();
			MDPart mdPart = mdb.GetPart(partNr);
			if (mdPart == null) return null;

			PointD location = GetSymbolLocation(page);

			string macroPath = mdPart.Properties.ARTICLE_GROUPSYMBOLMACRO.ToString();
			WindowMacro wm = new WindowMacro();
			wm.Open(macroPath, page.Project);
		    PointD[] points =	wm.GetBoundingBox();
			location.Y += Math.Abs(points[0].Y - points[1].Y) / 2;
			location.X += Math.Abs(points[0].X - points[1].X) / 2;
			StorableObject[] sos = new Insert().WindowMacro(
				wm,
				WindowMacro.Enums.RepresentationType.MultiLine,
				variant,
				page,
				location,
				Insert.MoveKind.Absolute,
				WindowMacro.Enums.NumerationMode.NumberPreferPrefix);
			
			CreateGroup(sos);

			foreach (StorableObject so in sos)
			{
				BoxedDevice bd = so as BoxedDevice;
				if (bd != null)
				{
					bd.Name = name;
					return bd;
				}
			}

			return null;
		}


		public static Function AddDoorArrangeSymbol(Project project, string name, string partNr, int variant)
		{
			name = name.Substring(0, name.IndexOf("-"));
			foreach (Page page in project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.PanelLayout) continue;

				foreach (Function f in page.Functions)
				{
					if (f.Name.StartsWith(name))
					{
						Function function = AddSymbol(f.Page, name, partNr, variant, WindowMacro.Enums.RepresentationType.ArticlePlacement);

						return function;
					}
				}
			}

			return null;
		}

		public static void CopyPages(List<Page> pageList, string name)
		{
			foreach (Page p in pageList)
			{
				if (p.Name.Contains(name) && p.PageType != DocumentTypeManager.DocumentType.CircuitSingleLine && p.PageType != DocumentTypeManager.DocumentType.Circuit)
				{
					Placement placement = p.CopyTo(p.Project.Pages[p.Project.Pages.Length - 1]);
				}
			}
		}

		public static void UndoLast()
		{
			new CommandLineInterpreter().Execute("XEsUndoAction");
		}

		public static void RedoLast()
		{
			new CommandLineInterpreter().Execute("XEsRedoAction");
		}

		public static void InsertSpecialSymbolVariant(Page page, string symbolName, int nVariant, PointD location)
		{
			string strSymbolLibName = "SPECIAL";

			SymbolLibrary oSymbolLibrary = new SymbolLibrary(page.Project, strSymbolLibName);
			Symbol oSymbol = new Symbol(oSymbolLibrary, symbolName);
			SymbolVariant oSymbolVariant = new SymbolVariant();

			oSymbolVariant.Initialize(oSymbol, nVariant);
			SymbolReference symbolReference = new SymbolReference();
			symbolReference.Create(page, oSymbolVariant);
			symbolReference.Location = location;
		}

		public static void ApplyPlotFrame(Project project, string fileName)
		{
			if (project == null)
			{
				MessageBox.Show("EPLAN Project가 없습니다....");
				return;
			}

			int count = 0;
			for (int i = 1; i < project.Pages.Length; i++)
			{
				try
				{
					project.Pages[i].Properties.PAGE_FORMPLOT = fileName;
					project.Pages[i].PlotFrame.CopyTo(project.Pages[i]);
					count++;
				}
				catch
				{ 
				}
			}

			MessageBox.Show(count.ToString() + " 개의 포맷이 적용되었습니다.");			
		}
	}
}
