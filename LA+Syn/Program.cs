using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.IO;
namespace LA
{
    //Lexical :D     
    class Program
    {
        static string line;

        /// <summary>
        /// ye hamari classes hain
        /// </summary>
        static string[] dataType = new string[] { "flp", "dig", "text", "chr", "bool" };
        static string[] incOp = new string[] { "++", "--" };
        static string[] assOp = new string[] { "+=", "-=", "*=", "/=", "=" };
        static string[] rOP = new string[] { "!=", "<=", ">=", "==", "!", "<", ">", };
        static string semicolon = ";";
        static string colon = ":";
        static string coma = ",";
        static string openRoundBracket = "(";
        static string closeRoundBracket = ")";
        static string condition = "condition";
        static string outt = "out";
        static string opencurlyBracket = "{";
        static string closecurlyBracket = "}";
        static int lineno = 1;

        /// <summary>
        /// hamri classes khatam
        /// </summary>
        /// <param name="args"></param>

        static int tokencounter = 0, linearcounter = 0;
        static string[,] Tokens = new string[1000, 3];
        static int[] LineNo = new int[1000];
        public static string inputPath = @"i:\muff.txt";
        static string outputPath = @"i:\errorlog.txt";
        static System.IO.StreamWriter newfile = new System.IO.StreamWriter(outputPath, false);


        static void Main(string[] args)
        {


            int counter = 0;// startOfWord, endOfWord;


            // Read the file and display it line by line.
            try
            {
                System.IO.StreamReader file =
                new System.IO.StreamReader(inputPath);

                int totalLines = File.ReadLines(inputPath).Count();
                while (!file.EndOfStream)
                {
                    if ((line = file.ReadLine()) != "")
                    {

                        //bool assopcheck = false;
                        //   startOfWord = 0;
                        counter = 0;
                        string word = "";


                        for (int i = 0; i < line.Length; i++)
                        {
                            counter++;

                            //comments check
                            if (line[i] == '/' && i < line.Length - 1 && line[i + 1] == '/')
                            {
                                lineno++;
                                break;
                            }





                            //string check
                            if (line[i] == '\"')
                            {
                                validation(word);
                                word = "";
                                if (i < line.Length - 1)
                                {
                                    int newcount = 0;

                                    if (line[i] == '\"' && line[i + 1] == '\"')
                                    {
                                        newfile.WriteLine("({0},nullStrConst,{1})", word, lineno);
                                        Tokens[tokencounter, 0] = word;
                                        Tokens[tokencounter, 2] = lineno.ToString();
                                        Tokens[tokencounter, 1] = "textConst";
                                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                        i++;
                                        continue;
                                    }

                                    i++;
                                    while (line[i] != '\"' && i < line.Length - 1)
                                    {

                                        newcount++;
                                        word += line[i];
                                        i++;
                                    }
                                    if (line[i] == '\"')
                                    {
                                        newfile.WriteLine("({0},StrConst,{1})", word, lineno);
                                        Tokens[tokencounter, 0] = word;
                                        Tokens[tokencounter, 2] = lineno.ToString();
                                        Tokens[tokencounter, 1] = "textConst";
                                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                        word = "";
                                        continue;
                                    }
                                    if (i < line.Length)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        validation(word);
                                        word = "";
                                        continue;
                                    }

                                }
                            }

                            //char check

                            if (line[i] == '\'')
                            {
                                validation(word);
                                word = "";
                                if (i < line.Length - 1 && line[i + 1] == '\'')
                                {
                                    newfile.WriteLine("({0},nullCharConst,{1})", word, lineno);
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "chrConst";

                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                    i++;
                                    continue;
                                }

                                if (i < line.Length - 2 && line[i + 2] == '\'')
                                {
                                    word += line[i + 1];
                                    i += 2;
                                    newfile.WriteLine("({0},CharConst,{1})", word, lineno);
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "chrConst";
                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                    word = "";
                                    continue;
                                }
                            }




                            if (line[i] == ' ' || line[i] == '\t' || line[i] == '(' || line[i] == ')' || line[i] == '{' || line[i] == '}' || line[i] == ';' || line[i] == '+' || line[i] == '=' || line[i] == '-' || line[i] == '*' || line[i] == '/' || line[i] == ';' || line[i] == ':' || line[i] == ',' || line[i] == '<' || line[i] == '>' || line[i] == '!')
                            {

                                validation(word);
                                word = "";

                            }

                            ////////////////////////////DOT check//////////////////////////////////

                            string character = "";
                            if (word.Length > 0)
                            {
                                character += word[word.Length - 1];
                                int n;
                                if (line[i] == '.' && !int.TryParse(character, out n))
                                {
                                    validation(word);
                                    word = "";
                                    continue;
                                }
                            }

                            if (i < line.Length - 1)
                            {
                                character = "";
                                character += line[i + 1];
                                int n;
                                if (line[i] == '.' && !int.TryParse(character, out n))
                                {

                                    word += line[i];
                                    validation(word);
                                    word = "";
                                    continue;
                                }
                                if (line[i] == '.' && int.TryParse(character, out n))
                                {
                                    word += line[i];
                                    i++;
                                    while (i < line.Length - 1 && line[i] >= '0' && line[i] <= '9')
                                    {
                                        word += line[i];
                                        i++;
                                    }
                                    if (i == line.Length - 1 && line[i] >= '0' && line[i] <= '9')
                                    {
                                        word += line[i];
                                    }
                                    validation(word);
                                    word = "";
                                    continue;
                                }
                            }

                            //float check
                            if (line[i] >= '0' && line[i] <= '9')
                            {
                                if (word != "")
                                {
                                    if (((word[word.Length - 1] >= 'a' && word[word.Length - 1] <= 'z') || (word[word.Length - 1] >= 'A' && word[word.Length - 1] <= 'Z')))
                                    {
                                        validation(word);
                                        word = "";
                                    }
                                    while (i < line.Length - 1 && line[i] >= '0' && line[i] <= '9')
                                    {
                                        word += line[i];
                                        i++;

                                    }
                                    if (i == line.Length - 1 && line[i] >= '0' && line[i] <= '9')
                                    {
                                        word += line[i];
                                        validation(word);
                                        continue;
                                    }
                                }
                                if (line[i] == '.')
                                {

                                    word += line[i];
                                    if (i < line.Length - 1)
                                    {
                                        i++;
                                        while (i < line.Length - 1 && line[i] >= '0' && line[i] <= '9')
                                        {
                                            word += line[i];
                                            i++;
                                        }
                                        if (i == line.Length - 1 && line[i] >= '0' && line[i] <= '9')
                                        {
                                            word += line[i];
                                        }

                                        validation(word);
                                        word = "";
                                        ///////

                                        //yahn bhnd hai:))
                                        //continue;
                                    }
                                }

                            }

                            //++ += validation

                            if (line[i] == '+')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '+')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;
                                }

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;

                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }


                            //-- -= validation

                            if (line[i] == '-')
                            {
                                if (i < line.Length - 1 && line[i + 1] == '-')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;

                                }

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }


                            //validation  *=

                            if (line[i] == '*')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }




                            //validation  /=

                            if (line[i] == '/')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;

                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }


                            //ROP validation 
                            if (line[i] == '!')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }

                            if (line[i] == '=')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;


                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }

                            if (line[i] == '<')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }

                            if (line[i] == '>')
                            {

                                if (i < line.Length - 1 && line[i + 1] == '=')
                                {

                                    word += line[i];
                                    word += line[i + 1];
                                    validation(word);
                                    i++;
                                    word = "";
                                    if (i == line.Length - 1)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    validation(word);
                                    word = "";
                                }
                            }

                            if (word == "." && !(line[i] >= '0' && line[i] <= '9'))
                            {
                                validation(word);
                                word = "";
                            }
                            if (!(line[i] == ' ' || line[i] == '\t' || line[i] == '(' || line[i] == ')' || line[i] == '{' || line[i] == '}' || line[i] == '=' || line[i] == '+' || line[i] == '-' || line[i] == '*' || line[i] == '/' || line[i] == ';' || line[i] == ':' || line[i] == '=' || line[i] == ',' || line[i] == '<' || line[i] == '>' || line[i] == '!'))
                                word += line[i];



                            if (line[i] == '(' || line[i] == ')' || line[i] == '{' || line[i] == '}' || line[i] == '=' || line[i] == '+' || line[i] == '-' || line[i] == '*' || line[i] == '/' || line[i] == ';' || line[i] == ':' || line[i] == '=' || line[i] == ',' || line[i] == '<' || line[i] == '>' || line[i] == '!')
                            {
                                validation(word);
                                word = "";
                                word += line[i];
                                validation(word);
                                word = "";
                            }


                            if (i == line.Length - 1)
                            {

                                validation(word);

                            }

                        }

                    }
                    lineno++;

                    //counter++;
                }


                Console.WriteLine("File Written to Path: " + outputPath);
                file.Close();
                newfile.Close();
            }

            catch (Exception e) { System.Console.WriteLine(e.Message); }
            SyntaxCheck scheck = new SyntaxCheck(Tokens, LineNo, tokencounter, linearcounter);
            if (scheck.Validate())
            {
                Console.WriteLine("NO Syntax Error");
            }
            else
            {
                Console.WriteLine("Syntax Error");
            }

        }

        private static void validation(string word)
        {
            bool match = false;
            // newfile.WriteLine(word + "=================== ");
            if (!String.IsNullOrWhiteSpace(word))
            {

                int counter = -1;

                //dataType keyword validation
                while (counter < dataType.Length - 1)
                {
                    counter++;
                    if (dataType[counter] == (word))
                    {
                        match = true;
                        newfile.WriteLine("({0},DT,{1})", word, lineno);
                        Tokens[tokencounter, 0] = word;
                        Tokens[tokencounter, 2] = lineno.ToString();
                        Tokens[tokencounter, 1] = "DT";
                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    }
                    if (match)
                        return;

                }


                for (int i = 0; i < assOp.Length; i++)
                {
                    if (word == assOp[i])
                    {
                        match = true;
                        newfile.WriteLine("({0},assOp,{1})", word, lineno);
                        Tokens[tokencounter, 0] = word;
                        Tokens[tokencounter, 2] = lineno.ToString();
                        Tokens[tokencounter, 1] = "AssOP";
                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    }
                }


                //if (stringvalidator(word))
                //{
                //    newfile.WriteLine("({0},StrConst,{1})", word, lineno);
                //    return;
                //}

                for (int i = 0; i < incOp.Length; i++)
                {
                    if (word == incOp[i])
                    {
                        match = true;
                        newfile.WriteLine(("({0},incOp,{1})"), word, lineno);
                        Tokens[tokencounter, 0] = word;
                        Tokens[tokencounter, 2] = lineno.ToString();
                        Tokens[tokencounter, 1] = "incOP";
                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    }
                    if (match)
                    {
                        return;
                    }
                }



                for (int i = 0; i < rOP.Length; i++)
                {
                    if (word == rOP[i])
                    {
                        match = true;
                        newfile.WriteLine("({0},rOp,{1})", word, lineno);
                        Tokens[tokencounter, 0] = word;
                        Tokens[tokencounter, 2] = lineno.ToString();
                        Tokens[tokencounter, 1] = "ROP";
                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    }
                    if (match)
                    {
                        return;
                    }

                }

                // ':'  ';' validation

                if (word == colon)
                {
                    newfile.WriteLine("({0},,{1})", colon, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = ":";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }

                if (word == semicolon)
                {
                    newfile.WriteLine("({0},,{1})", semicolon, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = ";";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }

                // '.' validation

                if (word == ".")
                {
                    newfile.WriteLine("({0},,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = ".";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }


                // '(' validaion

                if (word == openRoundBracket)
                {
                    newfile.WriteLine("({0},,{1})", openRoundBracket, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "(";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }
                if (word == closeRoundBracket)
                {
                    newfile.WriteLine("({0},,{1})", closeRoundBracket, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = ")";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }

                if (word == condition)
                {
                    newfile.WriteLine("({0},Con,{1})", condition, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "Condition";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }
                if (word == outt)
                {
                    newfile.WriteLine("({0},Out,{1})", outt, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "out";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }
                if (word == opencurlyBracket)
                {
                    newfile.WriteLine("({0},,{1})", opencurlyBracket, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "{";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }
                if (word == closecurlyBracket)
                {
                    newfile.WriteLine("({0},,{1})", closecurlyBracket, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "}";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }
                if (identifierValidator(word))
                {
                    if (word != "MOULD" && word != "con" && word != "Default" && word != "ret" && word != "begin" && word != "resume" && word != "fun" && word != "empty" && word != "Loop" && word != "either")
                    {
                        newfile.WriteLine("({0},ID,{1})", word, lineno);
                        Tokens[tokencounter, 0] = word;
                        Tokens[tokencounter, 2] = lineno.ToString();
                        Tokens[tokencounter, 1] = "ID";
                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    }
                    return;
                }
                if (intValidator(word))
                {
                    newfile.WriteLine("({0},IntConst,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "digConst";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }
                if (floatValidator(word))
                {
                    newfile.WriteLine("({0},FloatConst,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "flpConst";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }


                if (word == "+")
                {
                    newfile.WriteLine("({0},AOP,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "+";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }


                if (word == "-")
                {
                    newfile.WriteLine("({0},AOP,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "-";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }


                if (word == "*")
                {
                    newfile.WriteLine("({0},AOP,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "*";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }


                if (word == "/")
                {
                    newfile.WriteLine("({0},AOP,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "/";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }

                if (word == coma)
                {
                    newfile.WriteLine("({0},,{1})", word, lineno);
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = ",";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    return;
                }



                // if the word doesn't matches any off above
                if (match == false)
                    newfile.WriteLine(word + " has a error at line: " + lineno);



            }
        }

        private static bool identifierValidator(string word)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

            if (word == "MOULD")
            {
                Tokens[tokencounter, 0] = word;
                Tokens[tokencounter, 2] = lineno.ToString();
                Tokens[tokencounter, 1] = "MOULD";
                tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
            }
            else
                if (word == "con")
                {
                    Tokens[tokencounter, 0] = word;
                    Tokens[tokencounter, 2] = lineno.ToString();
                    Tokens[tokencounter, 1] = "con";
                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                }
                else

                    if (word == "Default")
                    {
                        Tokens[tokencounter, 0] = word;
                        Tokens[tokencounter, 2] = lineno.ToString();
                        Tokens[tokencounter, 1] = "Default";
                        tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                    }
                    else
                        if (word == "begin")
                        {
                            Tokens[tokencounter, 0] = word;
                            Tokens[tokencounter, 2] = lineno.ToString();
                            Tokens[tokencounter, 1] = "begin";
                            tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                        }
                        else
                            if (word == "ret")
                            {
                                Tokens[tokencounter, 0] = word;
                                Tokens[tokencounter, 2] = lineno.ToString();
                                Tokens[tokencounter, 1] = "ret";
                                tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                            }
                            else
                                if (word == "resume")
                                {
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "resume";
                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                }
                                else if (word == "fun")
                                {
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "fun";
                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                }
                                else if (word == "empty")
                                {
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "fun";
                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                }
                                else if (word == "Loop")
                                {
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "Loop";
                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                }
                                else if (word == "either")
                                {
                                    Tokens[tokencounter, 0] = word;
                                    Tokens[tokencounter, 2] = lineno.ToString();
                                    Tokens[tokencounter, 1] = "either";
                                    tokencounter++; LineNo[linearcounter] = lineno; linearcounter++;
                                }

            if (provider.IsValidIdentifier(word))
            {
                return true;
            }
            else if (word == "int" || word == "float" || word == "char" || word == "double" || word == "long" || word == "string" || word == "void" || word == "Main" || word == "new")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool intValidator(string word)
        {
            Regex intregex = new Regex("^[0-9]+$");

            if (intregex.IsMatch(word))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool floatValidator(string word)
        {
            Regex floatregex = new Regex(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?");

            if (floatregex.IsMatch(word))
            {
                return true;
            }
            else
            {
                return false;
            }
        }









        //public static bool stringvalidator(string word)
        //{
        //    Regex stringregex = new Regex("\"([^\"]*)\"");
        //    if (stringregex.IsMatch(word))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }


    class SyntaxCheck
    {
        int scope = -1;
        string[,] Token; int count = 0;
        int[] LineNo; int Linecount = 0;

        List<SymbolTable> varTable = new List<SymbolTable>();
        List<SymbolTable> funTable = new List<SymbolTable>();
        List<SymbolTable> classTable = new List<SymbolTable>();

        public SyntaxCheck(string[,] Token, int[] Line, int TotalCounts, int LineCount)
        {
            this.Token = new string[TotalCounts + 1, 3];
            this.Token = Token;
            this.Token[TotalCounts, 0] = "$";
            this.Token[TotalCounts, 1] = "$";
            this.Token[TotalCounts, 2] = "The end";
            this.LineNo = new int[LineCount + 1];
            this.LineNo = Line;
        }

        //Declaration
        public bool Dec()
        {
            string typeVP = "";
            string name = "";
            //  string scope="";

            if (Token[count, 1] == "DT")
            {
                typeVP = Token[count, 0];
                count++;
                if (Token[count, 1] == "ID")
                {
                    name = Token[count, 0];
                    if (decLookUpVarTable(name, scope))
                        insertInVarTable(name, typeVP, scope.ToString());
                    count++;
                    if (init(typeVP))
                    {
                        if (list(typeVP))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }



        //Initialization

        public bool init(string typeVP)
        {
            string asOP;
            if (Token[count, 1] == "AssOP")
            {
                asOP = Token[count, 0];
                count++;
                if (init2(typeVP, asOP))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }



        public bool init2(string typeVP, string asOP)
        {
            string typeVP2 = "";
            string name = "", constVp = "";
            if (Token[count, 1] == "ID")
            {
                name = Token[count, 0];
                if (vpLookUpVarTable(name, scope))
                    ///comp
                    typeVP2 = varType(name);
                typeVP = comparison(typeVP, typeVP2);
                if (typeVP == null)
                {
                    Console.WriteLine("TypeMismatch error");
                }

                count++;
                if (init(typeVP))
                {
                    return true;
                }
            }
            else if (constants(ref constVp))
            {
                //compS

                constVp = Token[count, 1];
                typeVP = comparison(typeVP, constVp);
                if (typeVP == null)
                {
                    Console.WriteLine("TypeMismatch error");
                }
                return true;
            }
            return false;
        }

        //List
        public bool list(string typeVP)
        {
            if (Token[count, 1] == ";")
            {
                count++;
                return true;
            }
            else if (Token[count, 1] == ",")
            {
                count++;
                if (init(typeVP))
                {
                    if (list(typeVP))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Constansts
        public bool constants(ref string constVP)
        {
            if (Token[count, 1] == "digConst")
            {
                constVP = Token[count, 1];
                count++;
                return true;
            }
            if (Token[count, 1] == "chrConst")
            {
                constVP = Token[count, 1];
                count++;
                return true;
            }
            if (Token[count, 1] == "flpConst")
            {
                constVP = Token[count, 1];
                count++;
                return true;
            }
            if (Token[count, 1] == "textConst")
            {
                constVP = Token[count, 1];
                count++;
                return true;
            }
            if (Token[count, 1] == "boolConst")
            {
                constVP = Token[count, 1];
                count++;
                return true;
            }

            return false;
        }

        //LooP
        public bool Loop()
        {
            if (Token[count, 1] == "Loop")
            {

                count++;
                if (Token[count, 1] == "(")
                {
                    count++;
                    if (LoopSt())
                    {
                        if (Token[count, 1] == ")")
                        {
                            count++;
                            if (Body())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool LoopSt()
        {

            if (Loop1())
            {
                return true;
            }
            else if (Loop2())
            {
                return true;
            }

            return false;
        }

        public bool Loop1()
        {
            string name = "", rOP = "", constVP = "", typeVP;
            if (Token[count, 1] == "ID")
            {
                typeVP = Token[count, 1];
                name = Token[count, 0];
                if (!(vpLookUpVarTable(name, scope)))
                    return false;
                count++;
                //if (LoopCond())
                //{
                //    if (constants())
                //{

                if (Token[count, 1] == "ROP")
                {
                    rOP = Token[count, 0];
                    count++;
                    if (ConstID(ref constVP))
                    {
                        //comp name rOP constVP

                        typeVP = comparison(typeVP, constVP);
                        if (typeVP == null)
                        {
                            Console.WriteLine("TypeMismatch error");
                        }

                        if (Token[count, 1] == ";")
                        {
                            count++;
                            if (Token[count, 1] == "ID")
                            {
                                name = Token[count, 0];
                                if (!(vpLookUpVarTable(name, scope)))
                                    return false;
                                string inop = "";
                                if (Token[count, 1] == "incOP")
                                {
                                    inop = Token[count, 1];
                                    //COmp or ye alag hoga baki comp se

                                    if (incComparison(name) == null)
                                        return false;

                                    
                                    count++;
                                    return true;
                                }
                                // }
                            }
                            //}
                            //}
                        }

                    }
                }
            } return false;
        }

        // ye nae h    
        public bool LoopCond()
        {
            if (LoopCond1())
            {
                return true;
            }


            else if (LoopCond1())
            {
                if (Token[count, 1] == "AssOP")
                {
                    count++;
                    return true;
                }

            }
            return false;
        }

        // ye  b nae h
        public bool LoopCond1()
        {
            if (Token[count, 1] == "AssOP")
            {
                count++;
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool Loop2()
        {
            string constVP = "";
            if (ConstID(ref constVP))
            {
                if (cond2(constVP))
                {
                    return true;
                }
            }
            return false;
        }

        public bool cond2(string constVP)
        {
            string typeVP = "";
            string rOP = "", constVP2 = "";
            if (Token[count, 1] == "ROP")
            {
                rOP = Token[count, 1];
                count++;
                if (ConstID(ref constVP2))
                {//comp constVP rop constVP2

                    typeVP = comparison(constVP, constVP2);
                    if (typeVP == null)
                    {
                        Console.WriteLine("TypeMismatch error");
                    }
                    return true;
                }
            }
            else return true;

            return false;
        }


        public bool Body()
        {
            if (Token[count, 1] == ";")
            {
                count++;
                return true;
            }
            else if (SingleST())
            {
                return true;
            }
            else if (Token[count, 1] == "{")
            {
                scope++;
                count++;
                if (MultiST() || Token[count, 1] == "}")
                {
                    if (Token[count, 1] == "}")
                    {
                        scope--;
                        count++;
                        return true;
                    }
                }
            }
            else return true;
            return false;
        }



        //Function Call
        public bool FunctionC()
        {
            string name = "";
            if (Token[count, 1] == "ID")
            {
                name = Token[count, 0];
                callLookUpfunTable(name, scope);
                count++;
                if (Token[count, 1] == "(")
                {
                    count++;
                    if (Paramc())
                    {
                        if (Token[count, 1] == ")")
                        {
                            count++;
                            if (Token[count, 1] == ";")
                            {
                                count++;
                                return true;
                            }

                        }
                    }
                }

            }
            return false;
        }

        public bool Paramc()
        {
            string constVP = "";

            if (ConstID(ref constVP))
            {
                if (param1())
                {
                    return true;
                }
            }
            else return false;
            return false;
        }

        public bool param1()
        {
            if (Token[count, 1] == ",")
            {
                count++;
                if (Paramc())
                {
                    return true;
                }
            }
            else return true;
            return false;
        }

        public bool Param(ref string a)
        {
            string type = "", name = "";
            if (Token[count, 1] == "DT")
            {
                a += Token[count, 0];
                type = Token[count, 0];
                count++;
                if (Token[count, 1] == "ID")
                {
                    name = Token[count, 0];
                    if (decLookUpVarTable(name, scope + 1))
                        insertInVarTable(name, type, Convert.ToString(scope + 1));
                    count++;
                    if (Param2(ref a))
                    {
                        return true;
                    }
                }
            }
            else return true;
            return false;
        }

        public bool Param2(ref string a)
        {
            if (Token[count, 1] == ",")
            {
                count++;
                if (Param(ref a))
                {
                    return true;
                }
            }
            else return true;

            return false;
        }

        public bool refType(ref string RT)
        {
            if (Token[count, 1] == "DT")
            {
                RT = Token[count, 0];
                count++;
                return true;
            }
            else if (Token[count, 1] == "empty")
            {
                RT = Token[count, 0];
                return true;
            }
            return false;
        }



        //Function Body
        public bool FunctionB()
        {
            string RT = "", a = "", funName = "";

            if (refType(ref RT))
            {

                if (Token[count, 1] == "ID")
                {
                    funName = Token[count, 0];
                    count++;
                    if (Token[count, 1] == "(")
                    {
                        count++;
                        if (Param(ref a))
                        {
                            if (Token[count, 1] == ")")
                            {
                                RT = a + "->" + RT;
                                if (!(decLookUpfunTable(funName, RT)))
                                    insertInFunTable(funName, RT, scope.ToString());
                                count++;
                                if (Token[count, 1] == "{")
                                {
                                    scope++;
                                    count++;
                                    if (Body())
                                    {
                                        if (Token[count, 1] == "}")
                                        {
                                            scope--;
                                            count++;
                                        }
                                    }
                                }
                                return true;
                            }
                        }
                    }

                }
            }
            return false;
        }


        //Either Or(if else)

        public bool Either()
        {
            string constVP = "";
            if (Token[count, 1] == "either")
            {
                count++;
                if (Token[count, 1] == "(")
                {
                    count++;
                    if (Cond(ref constVP))
                    {
                        if (Token[count, 1] == ")")
                        {
                            count++;
                            if (Body())
                            {
                                if (O_oor())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool O_oor()
        {
            if (Token[count, 1] == "oor")
            {
                count++;
                if (Body())
                {
                    return true;
                }
            }
            else return true;
            return false;
        }


        public bool Cond(ref string constVP)
        {
            string typeVP = "";
            string constVP2 = "", rOP;
            if (ConstID(ref constVP))
            {
                if (Token[count, 1] == "ROP")
                {
                    rOP = Token[count, 0];
                    count++;
                    if (ConstID(ref constVP2))
                    {
                        //comp constVP rOP constVP

                        typeVP = comparison(constVP, constVP2);
                        if (typeVP == null)
                        {
                            Console.WriteLine("TypeMismatch error");
                        }
                        return true;
                    }
                }
            }
            else if (ConstID(ref constVP))
            {
                return true;
            }
            return false;
        }

        public bool ConstID(ref string constVP)
        {
            string name = "";
            if (Token[count, 1] == "ID")
            {
                name = Token[count, 0];
                if (!(vpLookUpVarTable(name, scope)))
                    return false;
                constVP = Token[count, 0];
                constVP = varType(constVP);
                count++;
                return true;
            }
            else if (constants(ref constVP))
            {
                return true;
            }
            return false;
        }



        //Switch
        public bool Switch()
        {
            if (Token[count, 1] == "Condition")
            {
                string name = "", type = "";
                count++;
                if (Token[count, 1] == "(")
                {
                    count++;
                    if (Token[count, 1] == "ID")
                    {
                        name = Token[count, 0];
                        if (!(vpLookUpVarTable(name, scope)))
                            type = varType(name);
                        count++;
                        if (Token[count, 1] == ")")
                        {
                            count++;

                            if (Token[count, 1] == "{")
                            {
                                scope++;
                                count++;
                                if (SW_Body(type))
                                {
                                    if (Token[count, 1] == "Default")
                                    {
                                        count++;
                                        if (Token[count, 1] == ":")
                                        {
                                            count++;
                                            if (Body())
                                            {
                                                if (Token[count, 1] == "}")
                                                {
                                                    scope--;
                                                    count++;
                                                    return true;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                        }

                    }

                }

            }
            return false;
        }

        public bool SW_Body(string type)
        {
            string constVP = "";
            if (Token[count, 1] == "con")
            {
                count++;
                if (Token[count, 1] == ":")
                {
                    count++;
                    if (ConstID(ref constVP))
                    {
                        if (type == constVP) /////comp krna h yhn pe 
                            if (Body())
                            {
                                if (Token[count, 1] == "out")
                                {
                                    count++;
                                    if (Token[count, 1] == ";")
                                    {
                                        count++;
                                        if (Con1(type))
                                        {
                                            return true;
                                        }

                                    }

                                }
                            }
                    }

                }

            }
            return false;
        }

        public bool Con1(string type)
        {
            if (SW_Body(type))
            {
                return true;
            }
            else return true;
        }



        //Arrays

        public bool ArrDec()
        {
            string name = "", type = "", constVP = "";
            if (Token[count, 1] == "Adec")
            {
                if (Token[count, 1] == "DT")
                {
                    type = Token[count, 1];
                    count++;
                    if (Token[count, 1] == "ID")
                    {
                        name = Token[count, 0];
                        if ((decLookUpVarTable(name, scope)))
                            return false;
                        insertInVarTable(name, type, scope.ToString());

                        count++;
                        if (Token[count, 1] == "[")
                        {
                            count++;
                            if (Token[count, 1] == "intConst")
                            {
                                count++;
                                if (Token[count, 1] == "]")
                                {
                                    count++;
                                    if (ArrInit(ref constVP))
                                    {//comp
                                        if (ArrList(type))
                                        {
                                            return true;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool ArrInit(ref string constVP)
        {
            if (Token[count, 1] == "AssOP")
            {
                count++;
                if (Token[count, 1] == "{")
                {
                    scope++;
                    count++;
                    if (constants(ref constVP))
                    {
                        if (Token[count, 1] == "}")
                        {
                            scope--;
                            count++;
                            return true;
                        }

                    }

                }
            }
            else return true;
            return false;
        }


        public bool ArrList(string type)
        {
            string name = "", constVP = "";
            if (Token[count, 1] == ";")
            {
                count++;
                return true;
            }
            else if (Token[count, 1] == ",")
            {
                count++;
                if (Token[count, 1] == "ID")
                {
                    name = Token[count, 0];
                    if ((decLookUpVarTable(name, scope)))
                        return false;
                    insertInVarTable(name, type, scope.ToString());
                    count++;
                    if (Token[count, 1] == "[")
                    {
                        count++;
                        if (Token[count, 1] == "IntConst")
                        {
                            count++;
                            if (Token[count, 1] == "]")
                            {
                                count++;
                                if (ArrList(type))
                                {
                                    return true;
                                }

                            }
                        }
                    }

                }

            }
            return false;
        }



        //MOULD(Class)
        public bool Mould()
        {
            string name = "";
            if (Token[count, 1] == "MOULD")
            {
                count++;
                if (Token[count, 1] == "ID")
                {
                    if (decLookUpClassTable(name))
                        return false;
                    insertInClassTable(name);
                    count++;
                    if (Token[count, 1] == "{")
                    {
                        scope++;
                        count++;
                        while (All())
                        {

                            if (Token[count, 1] == "}")
                            {
                                scope--;
                                count++;
                                return true;
                            }

                        }
                    }
                }
            }

            return false;
        }


        //object

        public bool obj()
        {
            string className = "", objName="";
            if (Token[count, 1] == "Mobj")
            {
                count++;
                if (Token[count, 1] == "ID")
                {
                    className = Token[count, 0];
                    if (!callLookUpClassTable(className))
                        return false;
                    count++;
                    if (Token[count, 1] == "ID")
                    {
                        objName=Token[count,0];
                        decLookUpVarTable(objName,scope);
                        count++;
                        if (obj1())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool obj1()
        {
            if (Token[count, 1] == "(")
            {
                count++;
                if (Paramc())
                {
                    if (Token[count, 1] == ")")
                    {
                        if (Token[count, 1] == ";")
                        {
                            return true;
                        }
                    }
                }
                else if (Token[count, 1] == ")")
                {
                    if (Token[count, 1] == ";")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ObC()
        {
            string name="";
            if (Token[count, 1] == "Obc")
            {
                count++;
                if (Token[count, 1] == "ID")
                {
                    name=Token[count,0];
                    if (!vpLookUpVarTable(name,scope))
                        return false;
                    count++;
                    if (Token[count, 1] == "~")
                    {
                        count++;

                        if (ObC1())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool ObC1()
        {
            if (FunctionC())
            {
                return true;
            }
            return false;
        }

        public bool All()
        {
            if (Switch() || SingleST() || MultiST())
            {
                return true;
            }
            else if (Token[count, 1] == "fun")
            {
                count++;
                if (FunctionB() || FunctionC())
                {
                    return true;
                }
            }
            return false;
        }

        public bool SingleST()
        {
            string name = "";
            if (Loop())
            {
                return true;
            }
            else if (Either())
            {
                return true;
            }
            else if (Token[count, 1] == "ID")
            {
                name = Token[count, 0];
                if (vpLookUpVarTable(name, scope))
                    return false;
                count++;
                if (SingleST2())
                {
                    return true;
                }
            }
            else if (Token[count, 1] == "fun")
            {
                count++;
                if (FunctionC())
                {
                    return true;
                }
            }
            else if (Dec())
            {
                return true;
            }
            else if (ArrDec())
            {
                return true;
            }
            else if (ObC())
            {
                return true;
            }
            else if (obj())
            {
                return true;
            }

            return false;
        }


        public bool SingleST2()
        {
            //if (Token[count,1] == "(")
            //{
            //    count++;
            //    if (Param())
            //    {
            //        if (Token[count,1] == ")")
            //        {
            //            count++;
            //            if (Token[count,1] == ";")
            //            {
            //                count++;
            //                return true;
            //            }
            //        }

            //    }
            //}
            //else 
            if (Token[count, 1] == "AssOP")
            {
                count++;
                if (Exp())
                {
                    return true;
                }
            }

            return false;
        }


        public bool MultiST()
        {
            if (SingleST())
            {
                if (MultiST())
                {
                    return true;
                }
            }
            // else return true;
            return false;
        }



        //Expression
        public bool Exp()
        {
            string constVP = "";
            if (G(ref constVP))
            {
                if (Exp2(ref constVP))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Exp2(ref string constVP)
        {
            string rop = "";
            if (Token[count, 1] == "ROP")
            {
                rop = Token[count, 0];
                count++;
                if (G(ref constVP))
                {
                    if (Exp2(ref constVP))
                    {
                        return true;
                    }
                }
            }
            else return true;
            return false;
        }

        public bool G(ref string constVP)
        {
            if (H(ref constVP))
            {
                if (G2(ref constVP))
                {
                    return true;
                }
            }

            return false;
        }

        public bool G2(ref string constVP)
        {
            string constVP2 = "";
            string Pm = "";
            if (Token[count, 1] == "+" || Token[count, 1] == "-")
            {
                Pm = Token[count, 0];
                count++;
                if (H(ref constVP))
                {////////////////////////////////MD typ
                    if (G2(ref constVP2))
                    {
                        return true;
                    }
                }
            }
            else return true;
            return false;
        }

        public bool H(ref string constVP)
        {
            if (I(ref constVP))
            {
                if (H2(ref constVP))
                {
                    return true;
                }
            }

            return false;
        }

        public bool H2(ref string constVP)
        {
            string aOp = "", constVP2 = "";
            if (Token[count, 1] == "*" || Token[count, 1] == "/" || Token[count, 1] == "%")
            {
                aOp = Token[count, 0];
                count++;
                if (I(ref constVP2))
                {//comp constVP and constVP2 and save in constVP
                    if (H2(ref constVP))
                    {
                        return true;
                    }
                }
            }
            else return true;
            return false;
        }

        public bool I(ref string constVP)
        {
            string name = "", rOP = "";
            if (Token[count, 1] == "ID")
            {
                name = Token[count, 0];
                if (!(vpLookUpVarTable(name, scope)))
                    return false;
                constVP = varType(name);

                count++;
                return true;
            }
            else if (constants(ref constVP))
            {
                return true;
            }

            return false;
        }


        //Vlaidate
        public bool Validate()
        {
            bool t = false;
            while (Token[count, 1] != "$")
            {

                if (Token[count, 1] == "MOULD")
                {
                    if (Mould())
                    {
                        t = true;
                    }
                    else
                        t = false;
                }
                else return false;
            }
            if (t)
                return true;
            return false;
        }

        void insertInVarTable(string name, string type, string scope)
        {
            varTable.Add(new SymbolTable(name, type, scope));
        }

        void insertInFunTable(string name, string type, string scope)
        {
            funTable.Add(new SymbolTable(name, type, scope));
        }


        string varType(string name)
        {
            foreach (var item in varTable)
            {
                if (item.name == name)
                    return item.type;
            }
            return null;
        }

        bool decLookUpVarTable(string name, int s)
        {

            bool hasIt = false;
            foreach (var item in varTable)
            {
                if (item.name == name && item.scope == s.ToString())
                    hasIt = true;
            }

            if (hasIt)
                Console.WriteLine("Redeclaration error at line " + Token[count, 2]);
            return hasIt;
        }


     
        bool vpLookUpVarTable(string name, int s)
        {
            bool hasIt = false;
            foreach (var item in varTable)
            {
                for (int i = s; i > -1; i--)

                    if (item.name == name && item.scope == i.ToString())
                        hasIt = true;
            }

            if (!hasIt)
            {
                Console.WriteLine("Undeclared variable at line " + Token[count, 2]);

            }
            return hasIt;
        }




        bool decLookUpfunTable(string name, string type)
        {
            bool hasIt = false;
            foreach (var item in varTable)
            {
                if (item.name == name && item.type == type)
                    hasIt = true;
            }

            if (hasIt)
                Console.WriteLine("Redeclaration error at line " + Token[count, 2]);
            return hasIt;
        }

        bool callLookUpfunTable(string name, int s)
        {
            bool hasIt = false;
            foreach (var item in varTable)
            {
                for (int i = s; i > -1; i--)

                    if (item.name == name && item.scope == i.ToString())
                        hasIt = true;
            }

            if (!hasIt)
            {
                Console.WriteLine("Undeclared function at line " + Token[count, 2]);

            }
            return hasIt;
        }


        void insertInClassTable(string name)  /////////////////Class Table me insert k liye ////////////////////////
        {
            classTable.Add(new SymbolTable(name, null, null));
        }


        bool decLookUpClassTable(string name) /////////////////Class ki dec krte huay check krne k liye /////////////////////
        {
            bool hasIt = false;
            foreach (var item in varTable)
            {
                if (item.name == name)
                    hasIt = true;
            }

            if (hasIt)
                Console.WriteLine("Redeclaration error at line " + Token[count, 2]);
            return hasIt;
        }

        bool callLookUpClassTable(string name) ///////////// Class k object banate waqt check krne k liye ///////////////////
        {
            bool hasIt = false;
            foreach (var item in varTable)
            {

                if (item.name == name)
                    hasIt = true;
            }

            if (!hasIt)
            {
                Console.WriteLine("Undeclared Class at line " + Token[count, 2]);

            }
            return hasIt;
        }


        string comparison(string v1, string v2)        ////////////////////////// Compare two datatypes ////////////////////////////////
        {

            if (v1.StartsWith(v2.Substring(0, 3)))
            {
                return v1;
            }


            return null;
        }

        string incComparison(string s1)           ////////////// single (for ex a++ compare corne k liye ///////////////////////////
        {
            if (s1.Contains("dig") || s1.Contains("flp"))
            {
                return s1;
            }
            return null;
        }
    }
}


class SymbolTable
{
    public String name, type, scope;
    public SymbolTable(string a, string b, string c)
    {
        name = a;
        type = b;
        scope = c;

    }
}

