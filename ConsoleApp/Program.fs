// Learn more about F# at http://fsharp.org

open System

open RouletteStrategy.Entities
open RouletteStrategy.Strategy

[<EntryPoint>]
let main argv =
    printfn "Roulette Strategy Tester \n"

    // Settings
    let DOUBLE_ZERO = false
    let PREVIOUS_NUMBERS = 5
    let STOP_LOSS = 200
    let MAX_SLIPS = 200
    let INITIAL_BET = 2
    let SPEED = 1
    let AUTOPLAY = true        

    let random = new Random(DateTime.Now.Millisecond)  

    let getKey () = Console.ReadKey().KeyChar 

    let rec getBet suggestion = 

        if AUTOPLAY then 
            match suggestion with
            | Suggestion.D1_D2 -> 1, "D1 + D2"
            | Suggestion.D1_D3 -> 2, "D1 + D3"
            | Suggestion.D2_D3 -> 3, "D2 + D3"
            | _ -> 4, "skip"

        else
            let isSuggested x =
                if x = suggestion then " <-- suggested" else ""

            let output = "\n\tChoose a dozen: "
                       + "\n\t(1) 1-12  + 13-24 " + isSuggested Suggestion.D1_D2
                       + "\n\t(2) 1-12  + 25-36 " + isSuggested Suggestion.D1_D3
                       + "\n\t(3) 13-24 + 25-36 " + isSuggested Suggestion.D2_D3
                       + "\n\t(S) Skip          " + isSuggested Suggestion.None
                       + "\n\t(X) Stop          "
                       + "\n\t: "
            Console.Write output
                
            // Console.WriteLine (sprintf "your input: %c" input)
            match getKey() with
            | '1' -> 1, " 1-12 + 13-24"
            | '2' -> 2, " 1-12 + 25-36"
            | '3' -> 3, "13-24 + 25-36"
            | 'S' | 's' -> 4, "skip"
            | 'X' | 'x' -> 5, "stop" 
            | _ -> getBet suggestion

    let getNumber () = if DOUBLE_ZERO then random.Next(37) else random.Next(36)

    let playAgain balance slips = 
        balance > -STOP_LOSS 
        && slips < MAX_SLIPS

    // # Print functions
    
    let printSpin spinsCounter = Console.WriteLine (sprintf "\nSpin #%d" spinsCounter)
    let printNumbers (numbers:int list) = 
        let toPrint = 
            match numbers with 
            | [] -> ""
            | last::tail -> sprintf "(%d) %s" last (String.Join(", ", (Array.ofList tail.[..19])))        
        Console.WriteLine (sprintf "\n\tNumbers: %s ..."  toPrint) 
    let printChoice choice = Console.WriteLine( sprintf "\n\tYou choose \"%s\"" choice) 
    let printNumber number = Console.WriteLine( sprintf "\tExtracted number: %d" number)
    let printResult result = Console.WriteLine( match result with
                                                | Won -> sprintf  "\t** You Won! **"
                                                | Lost -> sprintf "\t** You Lost! **"   
                                                | Skip -> sprintf "\t* skip *"   
    )
   
    let getInitialNumbers n = 
        let rec addNumber numbers =
            match (numbers:int list).Length with 
            | x when x = n -> numbers
            | _ -> addNumber (getNumber()::numbers)
        addNumber []
                    
    let saveLogs logs = 
        System.IO.File.WriteAllLines(sprintf "log_%s.csv" (DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")), Array.ofList logs)


    // main loop
    let rec play previousNumbers spinsCounter balance logs =  
        printSpin spinsCounter       
        printNumbers previousNumbers

        let suggestion = RouletteStrategy.Strategy.strategy_2 previousNumbers PREVIOUS_NUMBERS

        let input, choice = getBet(suggestion)
        printChoice choice
       
        System.Threading.Thread.Sleep(1000/SPEED)
        let number = getNumber()
        printNumber number
        
        System.Threading.Thread.Sleep(1000/SPEED)

        let getBet input = match input with 
                           | 1 -> "1-12 + 13-24"
                           | 2 -> "1-12 + 25-36"
                           | 3 -> "13-24 + 25-36"
                           | _ -> "skip"

        let result = match number, input with
                     | _,4 -> Skip // skip
                     | 0,_ -> Lost
                     | n,1 when n >=1 && n<=11 -> Won 
                     | n,2 when n >=13 && n<=24 -> Won
                     | n,3 when n >=25 && n<=36 -> Won                     
                     | _ -> Lost  // double zero

        printResult result

        let log = sprintf "%d,%d,%s,%O" spinsCounter number (getBet input) result

        if playAgain balance spinsCounter 
        then play (number::previousNumbers) (spinsCounter+1) balance (log::logs)
        else saveLogs ("Spin,Number,Bet,Result"::List.rev(log::logs))

    let initialBalance = 0
    play (getInitialNumbers PREVIOUS_NUMBERS) 1 initialBalance []
         
    Console.WriteLine("END")

    0 // return an integer exit code



    //let values = (1,2,3)

    //let valueToLookFor = 2
    //let x1 = PREVIOUS_NUMBERS
    //let x2 = PREVIOUS_NUMBERS
    //let x3 = PREVIOUS_NUMBERS

    //let result =
    //    match values with
    //    | (x1,_,_) -> "pos 1"
    //    | (_,x2,_) -> "pos 2"
    //    | (_,_,x3) -> "pos 3"
    //    | _ -> "not found"


    //let result_2 = 
    //    match values with
    //    | (x,_,_) when x =valueToLookFor -> "pos 1"
    //    | (_,x,_) when x =valueToLookFor -> "pos 2"
    //    | (_,_,x) when x =valueToLookFor -> "pos 3"
    //    | _ -> "not found"

    //sprintf "%s" result
    //sprintf "%s" result_2

    //0