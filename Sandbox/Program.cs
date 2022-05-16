using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
    public class Counter
    {

        private  double? _percentage;
        public Counter(string name, int count)
        {
            Name = name;
            Count = count;
        }
         
        public string Name { get; }
        public int Count { get; private set; }
        public double GetPercent(int total) => _percentage ?? (_percentage = Math.Round(Count * 100.0 / total, 2 )).Value;
        

        public void AddExcess(double excess) => _percentage += excess;

        
    }

    public class CounterManager
    {
        public CounterManager(params Counter[] counters)
        {
            Counters = new List<Counter>(counters);
        }
        public List<Counter> Counters { get; set; }
        public int Total() => Counters.Sum(x => x.Count);
        public double TotalPercentage() => Counters.Sum(x => x.GetPercent(Total()));
        //
        //
        //   //this code was replaced by the above line of code
        //then it was converted to a lambda expression
        //public int Total()
        //{
        //    //Counters.Sum(x => x.Count);

        //    int total = 0;
        //    foreach (var c in Counters)
        //    {
        //        total += c.Count;
        //    }
        //    return total;
        //}


        //this is how the lambda function would ook like if i was a named function
        //public int Selector(Counter counter)
        //{
        //    return counter.Count;
        //}

        public void AnnounceWinner()
        {
            // int total = yes.Count + no.Count + maybe.Count;

           

            var excess = Math.Round(100 - TotalPercentage(), 2);

            Console.WriteLine($"Excess: {excess}");

            var biggestAmountOfVotes = Counters.Max(x => x.Count);

            var winners = Counters.Where(x => x.Count == biggestAmountOfVotes).ToList();

          if (winners.Count == 1)
            {
                var winner = winners.First();
                winner.AddExcess(excess);
                Console.WriteLine($"{winner.Name} Won");
            } else
            {           

                if (winners.Count != Counters.Count)
                {
                    var lowestAmountOfVotes = Counters.Min(x => x.Count);
                    var losers = Counters.First(x => x.Count == lowestAmountOfVotes);
                    losers.AddExcess(excess);
                    
                }
               // Console.WriteLine($"{losers.Name} Lost!");
                Console.WriteLine(string.Join(" -Draw- ", winners.Select(x => x.Name)));
                             
                
            }

            foreach (var c in Counters)
            {
                Console.WriteLine($"{c.Name} Count :{c.Count}, Percentage is {c.GetPercent(Total())}%");
            }

            Console.WriteLine($"Total Percentage: {Math.Round(TotalPercentage(), 2)}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var yes = new Counter("Yes", 6);
            var no = new Counter("No", 2);
            var maybe = new Counter("Maybe", 1);
            var hopefuly = new Counter("Hopefully", 1);

            var manager = new CounterManager( yes, no, maybe, hopefuly);
            
            manager.AnnounceWinner();
          
        }
    }
}
