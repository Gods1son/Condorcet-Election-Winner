using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
    /*Assignment: Determine a condorcet winner from a file
    Group 6: Assignment 11
              Harsimar Ahluwalia
              Najam Ahmad
              Tolulope Ibiyode
              Gurpreet Kaur
              Pablo Martinez
              Maryna Salii
              Chandana Bolanthuru*/
namespace Final_Condorcet_Winner
{
    class VM:INotifyPropertyChanged
    {
        public string Number { get; set; }
        public int Count { get; set; }
        public List<string> Result
        {
            get { return _result; }
            set { _result = value; NotifyPropertyChanged(); }
        }
        List<string> _result = new List<string>();
        public bool WinnerKnown { get; set; }
        public bool ReadAgain { get; set; }
        public const string FILE = "votes.txt";
        StreamReader read = new StreamReader(FILE);
        public VM()
        {
           Winner();
        }    
        public void Winner()
        {
            try
            {
                ReadAgain = true;
                do
                {
                    Count++;
                    //reading the lines and splitting it into rows and columns
                    string[] readTestFile = read.ReadLine().ToString().Split(' ');

                    //splitting the first line of the input to know number of ballots and candidates
                    int Ballots = int.Parse(readTestFile[0]);
                    int Candidates = int.Parse(readTestFile[1]);

                    //creating two-dimensional arrays to store the ballots
                    int[][] Votes = new int[Ballots][];

                    // Ballots == 0 && Candidates == 0 shows the end of the input
                    if (Ballots == 0 && Candidates == 0)
                    {
                        ReadAgain = false;
                    }
                    else
                    {
                        //filling each row of the array with number of candidates without exact values
                        for (int i = 0; i < Ballots; i++)
                            Votes[i] = new int[Candidates];

                        for (int i = 0; i < Ballots; i++)
                        {
                            string[] otherLines = new string[Candidates];
                            string otherLine = read.ReadLine().ToString();
                            //splits each line into array to put the values into the columns
                            otherLines = otherLine.Split(' ');
                            for (int v = 0; v < otherLines.Length; v++)
                            {
                                Votes[i][v] = int.Parse(otherLines[v]);
                            }
                        }
                        //another array to store each candidates and adds the index of each candidates through the array
                        int[] SumOfPosition = new int[Candidates];
                        for (int v = 0; v < Candidates; v++)
                        {
                            //looping through the rows of the Voters array and getting the index of each candidate
                            for (int x = 0; x < Ballots; x++)
                            {
                                SumOfPosition[v] += Array.IndexOf(Votes[x], v);
                            }
                        }
                        // saving sum of index and the candidates into dictionary
                        Dictionary<int, int> Election = new Dictionary<int, int>();
                        foreach (var figure in SumOfPosition)
                        {
                            if (Election.ContainsKey(figure))
                                Election[figure]++;
                            else
                                Election[figure] = 1;
                        }
                        WinnerKnown = false;
                        //value that is greater than one indicates a tie between candidates sum of index
                        foreach (var condition in Election)
                            if (condition.Value > 1)
                            {
                                Number = "Case " + Count + ": " + "No Condorcet Winner";
                                _result.Add(Number);
                                WinnerKnown = true;
                            }
                        if (WinnerKnown == false)
                        {
                            //the winning candidate is the candidate with the minimum sum of index
                            int Find = Array.IndexOf(SumOfPosition, SumOfPosition.Min());
                            Number = "Case " + Count + ": " + Find.ToString() + " is the winner";
                            _result.Add(Number);
                        }
                    }
                } while (ReadAgain == true);
            }
            catch(Exception)
            {
                System.Windows.MessageBox.Show("Please check the format of input text file");
            }
      }      
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
