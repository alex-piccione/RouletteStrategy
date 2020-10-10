namespace RouletteStrategy

module Entities =

    type Area =
        | Zero
        //| DoubleZero
        | D1 // 1 to 12
        | D2 // 13 to 24
        | D3 // 25 to 36


    type Bet =
        | Skip
        | D1_D2   //  1-12 + 13-24
        | D1_D3   //  1-12 + 25-36
        | D2_D3   // 23-24 + 25-36


    type Action =
        | Bet of Bet
        | Skip
        | Stop


    type SpinResult =
        | Won
        | Lost
        | Skip

        
    type GameResult = 
        { Spins:int; Wins:int; Losses:int; WinLostRatio:float }

        static member New () = 
            { Spins=0; Wins=0; Losses=0; WinLostRatio=0.0 }
            