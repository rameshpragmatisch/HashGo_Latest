using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Enum
{
    public enum MealItOptions
    {
        None = 0,
        AlaCarte,
        MealIt,
    }

    public class MealItOptionHelper
    {
        public static string AlaCarte = "ALA CARTE";
        public static string MealIt = "MEAL IT";

        public static string GetComboOptionName(MealItOptions options)
        {
            switch(options) 
            {
                case MealItOptions.AlaCarte: return AlaCarte;
                case MealItOptions.MealIt: return MealIt;
                default:
                return string.Empty;
            }
        }

        public static string GetWorkFlowByComboName(string comboTypeName)
        {
            switch (comboTypeName)
            {
                case "MEAL IT": return Pages.MealItWorkFlow.ToString();
                case "ALA CARTE": return Pages.AlacarteWorkFlow.ToString();
                default:
                    return Pages.None.ToString();
            }
        }

        public static MealItOptions GetComboOptionByName(string comboTypeName)
        {
            switch (comboTypeName)
            {
                case "MEAL IT": return MealItOptions.MealIt;
                case "ALA CARTE": return MealItOptions.AlaCarte;
                default:
                    return MealItOptions.None;
            }
        }

    }
}
