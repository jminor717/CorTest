using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORMobileApp.Database;
using CORMobileApp.ViewModels;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CORMobileApp.Views {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class InvenToryPage : CarouselPage {
        InstrumentsViewModel viewModel;
        Instrument startWith;
        public InvenToryPage (InstrumentsViewModel model,Instrument clicked) {
            viewModel = model;
            startWith = clicked;
            InitializeComponent ();
            try {
                renderPages();
            }
            catch (Exception err) {
                Debug.WriteLine(err);
            }
        }

        private void renderPages() {
            Thickness padding = new Thickness();
            switch (Device.RuntimePlatform) {
                case (Device.iOS): { padding = new Thickness(10, 10, 10, 10); break; }
                case (Device.Android): { padding = new Thickness(10, 10, 10, 10); break; }
            }
            ContentPage selected = new ContentPage { };
            foreach (Instrument instru in viewModel.SavedInstruments) {
                var stack = new StackLayout {
                    Children = {
                        new Label {
                            Text = instru.Name, FontSize = Device.GetNamedSize (NamedSize.Large, typeof (Label)), TextColor = Color.White
                        }
                    }
                };
                foreach (dynamic mod in instru.InstrumentData.modules) {
                    stack.Children.Add(
                        new Label {
                            Text = mod.Type+" "+mod.Side + " (" + mod.InstrumentSerialNumber + ") ",
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            TextColor = Color.White
                        });
                    stack.Children.Add(new BoxView() { Color = Color.White, WidthRequest = 100, HeightRequest = 2 });
                    //JArray arr = mod.Devices;
                    bool hasBulk=false;
                    foreach (dynamic inventoryItem in mod.Inventory) {
                        string code = inventoryItem.ConsumableCode;
                        if (code.IndexOf("BULK_CHANNEL")!=-1) { hasBulk = true; continue; }//if this module has bulk diluent inventory we will display it later
                        stack.Children.Add(human_readable_consumable(inventoryItem));
                    }
                    if (hasBulk) {
                        stack.Children.Add(makeDiluentDisplay(mod));
                    }
                }
                var ContentPage = new ContentPage {
                    Padding = padding,
                    BackgroundColor = Color.FromHex("#4c637b"),
                    Content = new ScrollView {
                        Content = stack
                    }
                };
                Children.Add(ContentPage);
                if (instru.Name == startWith.Name) { selected = ContentPage; Debug.WriteLine(instru.Name); }
            }

            this.SelectedItem = selected;
        }

        private Element makeDiluentDisplay(dynamic module) {


            StackLayout ontent = new StackLayout {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    new Label {
                        Text = "Bulk Diluent Aliquots",
                        FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.Bold
                    },
                }
            };
            foreach (dynamic inventoryItem in module.Inventory) {
                string code = inventoryItem.ConsumableCode;
                if (code.IndexOf("BULK_CHANNEL") == -1) { continue; }//if not a bulk diluent continue
                string assayName = inventoryItem.AssayName;
                int avalableqt = inventoryItem.AvailableQuantity;
                Color ccr = Color.Green;
                //other inventory errors ocor at low volumes
                if      (avalableqt / 400 < .1) { ccr = Color.Red; }//"images/state-error.svg"; 
                else if (avalableqt / 400 < .4) { ccr = Color.Yellow; }//"images/state-warning.svg"; 
                
                StackLayout Dill = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        new Label {
                            Text = Bulk_chanel_name(inventoryItem)
                        },
                        new Label {
                            Text = assayName
                        },
                        new StackLayout {
                            Orientation = StackOrientation.Horizontal,HorizontalOptions = LayoutOptions.EndAndExpand,
                            Children = {
                                 new Label {
                                    Text = avalableqt.ToString(), TextColor = ccr, FontAttributes = FontAttributes.Bold
                                },
                                new Label {
                                    Text ="/400"
                                }
                            }
                        }
                    }
                };

                ontent.Children.Add(new BoxView() { Color = Color.Black, WidthRequest = 100, HeightRequest = 2 });
                ontent.Children.Add(Dill);
                
            }
            Frame container = new Frame {
                Padding = new Thickness(5,2,5,2),
                Content = ontent
            };
            return container;
        }





        private Element generate_inventory_card(dynamic element, string tytle, int outofnum, string outovText ) {
            int avalableqt = element.AvailableQuantity;
            Color ccr = Color.Green;
            if (tytle.IndexOf("Waste")!=-1) {//waste errors ocor at high volumes
                if      (avalableqt / outofnum > .9) { ccr = Color.Red; } 
                else if (avalableqt / outofnum > .6) { ccr = Color.Yellow; }
            } else {//other inventory errors ocor at low volumes
                if      (avalableqt / outofnum < .1) { ccr = Color.Red; }//"images/state-error.svg"; 
                else if (avalableqt / outofnum < .4) { ccr = Color.Yellow; }//"images/state-warning.svg"; 
            }
            var QuantityOutove= new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children = {
                        new Label {
                            Text = avalableqt.ToString(), FontSize = Device.GetNamedSize (NamedSize.Default, typeof (Label)), TextColor = ccr, FontAttributes = FontAttributes.Bold
                        },
                        new Label {
                            Text = "/"+outofnum.ToString()+" "+outovText, FontSize = Device.GetNamedSize (NamedSize.Default, typeof (Label)), TextColor = Color.Black
                        }
                    }
            };

            return new Frame {
                Padding = 0,
                Content = new StackLayout {
                    BackgroundColor = Color.White,
                    Children = {
                        new Label {
                            Text = tytle, FontSize = Device.GetNamedSize (NamedSize.Default, typeof (Label)), TextColor = Color.Black, FontAttributes = FontAttributes.Bold
                        },
                        QuantityOutove
                    }
                },
            };
        }


        private Element human_readable_consumable(dynamic element) {
            string code = element.ConsumableCode;
            switch (code) {
                case ("TipsDrawerTips"): {
                        return generate_inventory_card(element, "1000 ul Tip Drawrs", 180, " TESTS");
                }
                case ("ConsumableDrawers"): {
                        return generate_inventory_card(element, "HPV Extraction Drawrs", 180, " TESTS");
                }
                case ("Solid Waste"): {
                        return generate_inventory_card(element, "Solid Waste", 540, " TESTS");
                }
                case ("Liquid Waste"): {
                        return generate_inventory_card(element, "Liquid Waste", 1840, " TESTS");
                }
                case ("Reagent1"): {
                        return generate_inventory_card(element, "HPV Extraction Trough - L", 540, " TESTS");
                }
                case ("Reagent2"): {
                        return generate_inventory_card(element, "HPV Extraction Trough - R", 540, " TESTS");
                }
                case ("PositiveHPVControls"): {
                        return generate_inventory_card(element, "HPV Pos(+) Controls", 62, " CONTROLS");
                }
                case ("NegativeHPVControls"): {
                        return generate_inventory_card(element, "HPV Neg(-) Controls", 62, " CONTROLS");
                }
                case ("EmptyPCapTubes"): {
                        return generate_inventory_card(element, "Molecular Aliquot Tubes", 378, " ALIQUOTS");
                }
                case ("1000uLPipetteTips"): {
                        return generate_inventory_card(element, "1000 ul Tip Trays", 567, " ALIQUOTS");
                }
                case ("CombinedWaste"): {
                        return generate_inventory_card(element, "Waste Levels", 960, " ALIQUOTS");
                }
                default: {
                        return null;
                }
            }
        }

        private string Bulk_chanel_name(dynamic element) {
            string code = element.ConsumableCode;
            switch (code) {
                case ("BULK_CHANNEL_1"): {
                        return "1 - ";
                    }
                case ("BULK_CHANNEL_2"): {
                        return "2 - ";
                    }
                case ("BULK_CHANNEL_3"): {
                        return "3 - ";
                    }
                case ("BULK_CHANNEL_4"): {
                        return "4 - ";
                    }
                case ("BULK_CHANNEL_5"): {
                        return "5 - ";
                    }
                case ("BULK_CHANNEL_6"): {
                        return "6 - ";
                    }
                case ("BULK_CHANNEL_7"): {
                        return "7 - ";
                    }
                case ("BULK_CHANNEL_8"): {
                        return "8 - ";
                    }
                default: {
                        return "";
                    }
            }
        }
    }
}