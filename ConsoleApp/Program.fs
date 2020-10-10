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
    let STOP_LOSS = 200.0
    let MAX_SLIPS = 1000
    let INITIAL_BET = 2
    let SPEED = 1000
    let AUTOPLAY = true        

    let random = new Random(DateTime.Now.Millisecond)  

    let getKey () = Console.ReadKey().KeyChar 

    let rec getAction (suggestion:Bet) :Action = 

        if AUTOPLAY then
            match suggestion with            
            | Bet.Skip -> Action.Skip
            | _ -> Action.Bet(suggestion)
        else
            let isSuggested bet = if suggestion = bet then " <-- suggested" else ""

            let output = "\n\tChoose a dozen: "
                       + "\n\t(1) 1-12  + 13-24 " + isSuggested Bet.D1_D2
                       + "\n\t(2) 1-12  + 25-36 " + isSuggested Bet.D1_D3
                       + "\n\t(3) 13-24 + 25-36 " + isSuggested Bet.D2_D3
                       + "\n\t(S) Skip          " + isSuggested Bet.Skip
                       + "\n\t(X) Stop          "
                       + "\n\t: "
            Console.Write output
                
            match getKey() with
            | '1' -> Bet Bet.D1_D2
            | '2' -> Bet Bet.D1_D3
            | '3' -> Bet Bet.D2_D3
            | 'S' | 's' -> Bet Bet.Skip
            | 'X' | 'x' -> Action.Stop
            | _ -> getAction suggestion  // ask again

    let getNumber () = if DOUBLE_ZERO then random.Next(37) else random.Next(36)

    let playAgain balance slips = 
        balance > -STOP_LOSS 
        && slips <= MAX_SLIPS

    // # Print functions
    
    let printSpin spinsCounter = Console.WriteLine (sprintf "\nSpin #%d" spinsCounter)
    let printNumbers (numbers:int list) = 
        let toPrint = 
            match numbers with 
            | [] -> ""
            | last::tail -> sprintf "(%d) %s" last (String.Join(", ", (Array.ofList tail.[..19])))        
        Console.WriteLine (sprintf "\n\tNumbers: %s ..."  toPrint) 

    let printAction action = Console.WriteLine( sprintf "\n\tYou choose \"%O\"" action) 
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

    let saveResult result =
        let result = sprintf "%O Spins:%d Wins:%d Losses:%d InitialBalance:%f FinalBalance:<not calculated> \n" 
                         (DateTime.Now.ToString("o")) // DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")
                         result.Spins
                         result.Wins
                         result.Losses
                         result.InitialBalance
                         //result.FinalBalance
        System.IO.File.AppendAllText("Results.log", result)


    // main loop
    let rec play previousNumbers spinsCounter result logs =  
        printSpin spinsCounter       
        printNumbers previousNumbers

        let suggestion = strategy_2 previousNumbers PREVIOUS_NUMBERS

        let action = getAction suggestion
        printAction action

        if action = Stop then
            result, logs
        else       
            System.Threading.Thread.Sleep(1000/SPEED)
            let number = getNumber()
            printNumber number
        
            System.Threading.Thread.Sleep(1000/SPEED)
            let betResult = match number, action with
                            | _,Action.Skip -> SpinResult.Skip 
                            | 0,_ -> Lost
                            | n, Action.Bet(bet) ->
                                match n with
                                | n when n >= 1 && n<=12 && List.contains bet <| [Bet.D1_D2; Bet.D1_D3] -> Won  
                                | n when n >=13 && n<=24 && List.contains bet <| [Bet.D1_D2; Bet.D2_D3] -> Won  
                                | n when n >=25 && n<=36 && List.contains bet <| [Bet.D1_D3; Bet.D2_D3] -> Won  
                                | _ -> Lost                    
                            | _ -> Lost  // double zero

            printResult betResult

            let log = sprintf "%d,%d,%O,%O" spinsCounter number action result
            let newResult = { result with
                                         Spins = spinsCounter
                                         Wins = result.Wins + if betResult = Won then 1 else 0
                                         Losses = result.Losses + if betResult = Lost then 1 else 0
                                         //FinalBalance = 
            }

            if playAgain result.FinalBalance spinsCounter 
            then play (number::previousNumbers) (spinsCounter+1) newResult (log::logs)
            else 
                result, logs

    let gameResult,logs = play (getInitialNumbers PREVIOUS_NUMBERS) 1 (GameResult.New()) []
    
    saveLogs ("Spin,Number,Bet,Result"::(List.rev logs))
    saveResult gameResult
         
    Console.WriteLine("END")

    0 // return an integer exit code

