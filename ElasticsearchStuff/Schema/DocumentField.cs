using System;
using System.Collections.Generic;

namespace ElasticsearchStuff.Schema
{
    public enum FieldType
    {
        String,
        Integer,
        Float,
        Date
    }

    public abstract class DocumentField
    {
        public static string FIELD_NAME = "fieldName";
        public static string TYPE = "type";

        private readonly string fieldName;
        private readonly FieldType type;

        public DocumentField(string fieldName, FieldType type)
        {
            this.fieldName = fieldName;
            this.type = type;
        }

        public string FieldName { get { return fieldName; } }
        public FieldType FieldType { get { return type; } }
    }

    public class StringField : DocumentField
    {
        public static string LENGTH = "length";
        public static string POSSIBLE_VALUES = "possibleValues";

        private readonly int length;
        private readonly List<string> possibleValues;

        public StringField(string fieldName, FieldType type, int length, List<string> possibleValues)
            :base(fieldName, type)
        { 
            this.length = length;
            this.possibleValues = possibleValues;
        }

        public int Length { get { return length; } }
        public List<string> PossibleValues { get { return possibleValues; } }
      
    }

    public class IntegerField : DocumentField
    {
        public static string MIN = "min";
        public static string MAX = "max";

        private readonly int min;
        private readonly int max; 

        public IntegerField(string fieldName, FieldType type, int min, int max)
            : base(fieldName, type)
        { 
            this.min = min;
            this.max = max;
        }

        public int Min { get { return min; } }

        public int Max { get { return max; } }
    }

    public class FloatField : DocumentField
    {
        public static string MIN = "min";
        public static string MAX = "max";

        private readonly float min;
        private readonly float max;

        public FloatField(string fieldName, FieldType type, float min, float max)
            : base(fieldName, type)
        { 
            this.min = min;
            this.max = max;
        }
        
        public float Min { get { return min; } }

        public float Max { get { return max; } }
    }
    public class DateField : DocumentField
    {
        public static string UPPER = "upper";
        public static string LOWER = "lower";

        private readonly DateTime upper;
        private readonly DateTime lower;

        public DateField(string fieldName, FieldType type, DateTime upper, DateTime lower)
            : base(fieldName, type)
        { 
            this.upper = upper;
            this.lower = lower;
        }

        public DateTime Upper { get { return upper; } }

        public DateTime Lower { get { return lower; } }

    }
}
