using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Advent04
{
    class Program
    {

        private static readonly Regex hclRegex = new Regex("^#[0-9a-f]{6}$");
        private static readonly Regex pidRegex = new Regex("^[0-9]{9}$");
        private static readonly Regex eclRegex = new Regex("^(amb|blu|brn|gry|grn|hzl|oth)$");
        private static (string key,Func<string, bool> validator)[] MandatoryPassportFields = { 
            ("byr", ValidateRange(1920,2002)),       
            ("iyr", ValidateRange(2010,2020)),
            ("eyr", ValidateRange(2020,2030)),
            ("hgt", ValidateHeight),
            ("hcl", value => hclRegex.IsMatch(value)),
            ("ecl", value => eclRegex.IsMatch(value)),                                
            ("pid", value => pidRegex.IsMatch(value)), 
        };
        private const string optionnalPassportField = "cid";

        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt").ToArray();
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        private static int Part1(string[] lines)
        {
            int result = 0;
            StringBuilder passportBuilder = new StringBuilder();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    var passport = passportBuilder.ToString();
                    if (MandatoryPassportFields.All(f => passport.Contains($"{f.key}:"))) 
                        result++;
                    passportBuilder.Clear();
                    continue;
                }
                passportBuilder.AppendLine(line);
            }
            return result;
        }

        private static int Part2(string[] lines)
        {
            int result = 0;
            List<(string key,string value)> passportFields = new List<(string, string)>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    passportFields.AddRange(ParsePassport(line));
                    continue;
                }
                
                if(MandatoryPassportFields.All(field => passportFields.Any(passportFields => field.key == passportFields.key && field.validator(passportFields.value))))        
                {
                    result++;
                }

                passportFields.Clear();                
            }
            return result;
        }
        
        private static (string key, string value)[] ParsePassport(string passport) => passport.Trim()
                .Split(' ')
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .Select(o => o.Split(":"))
                .Select(o => (o[0],o[1]))
                .ToArray();

        
        private static Func<string,bool> ValidateRange(int borneInf, int borneSupp){
            var validRange = Enumerable.Range(borneInf, borneSupp - borneInf + 1);
            return value => {
                int intValue = int.Parse(value);
                return validRange.Contains(intValue);
            };          
        }

        private static bool ValidateHeight(string value){
            if(value.EndsWith("cm"))
                return ValidateRange(150, 193)(value.Replace("cm",""));
            if(value.EndsWith("in"))
                return ValidateRange(59, 76)(value.Replace("in",""));
            return false;
        }
    }

}
