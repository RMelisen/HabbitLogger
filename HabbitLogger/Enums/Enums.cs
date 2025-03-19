using System.ComponentModel;
using System.Reflection;

namespace HabbitLogger.Enums
{
    internal enum MainMenuOption
    {
        Insert,
        Delete,
        Update,
        View,
        Quit
    }

    internal enum ViewTablesOption
    {
        Habbits, 
        [Description("Unit of measures")]
        UnitOfMeasures,
        [Description("Habbit occurences")]
        HabbitOccurences,
        Back
    }

    internal static class EnumUtils
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }
    }
}
