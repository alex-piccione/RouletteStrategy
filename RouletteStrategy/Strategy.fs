namespace RouletteStrategy

open Entities

module Strategy =

    let private getArea number =
        match number with 
        | n when n>= 1 && n<=12 -> Area.D1
        | n when n>=13 && n<=24 -> Area.D2
        | n when n>=25 && n<=36 -> Area.D3
        | _ -> Area.Zero

    let private missedInARow (numbers:int list) in_a_row =

        let rec checkPrevious numbers counters =
            // counters contains for how many time the Area D1, D2 and D3 have NOT appeared respectively
            let (d1,d2,d3) = counters

            match counters with
            | x,_,_ when x=in_a_row -> Area.D1
            | _,x,_ when x=in_a_row -> Area.D2
            | _,_,x when x=in_a_row -> Area.D3
            | _ ->
                match numbers with
                | [] -> Area.Zero // Area.None // not enough data
                | n::tail ->  
                    let newCounters = 
                        match getArea n with
                        | Area.D1 -> (0, d2+1, d3+1)
                        | Area.D2 -> (d1+1, 0, d3+1)
                        | Area.D3 -> (d1+1, d2+1, 0)
                        | _ -> (d1+1, d2+1, d3+1)

                    checkPrevious tail newCounters  

        checkPrevious numbers.[..in_a_row] (0,0,0)


    let fate = new System.Random(System.DateTime.Now.Millisecond)
    let strategy_random = fun (previousNumbers:int list) in_a_row ->
        match fate.Next(1,5) with        
        | 1 -> Bet.D1_D2
        | 2 -> Bet.D1_D3
        | 3 -> Bet.D2_D3
        | _ -> Bet.Skip


    let strategy_2 = fun (previousNumbers:int list) in_a_row ->
        // if there is a dozen that didn't appered in the last "in_a_row" spins, suggest the other 2 dozens (bet against it)
        // it is not perfect because it does not consider a "second place" while choosing the other 2 dozens 
        // but it is a so very rare event both 2 dozens not appear in a row for the same number of slips that can be ignored.
               
        let notAppearedArea = missedInARow previousNumbers in_a_row

        match notAppearedArea with
        | Area.D1 -> Bet.D2_D3  
        | Area.D2 -> Bet.D1_D3
        | Area.D3 -> Bet.D1_D2
        | _ -> Bet.Skip



    let strategy_3 = fun previousNumbers in_a_row ->
        // if in the last "in_a_row" spins there is an Area that does not appeared, bet on it

        let missedInARow = missedInARow previousNumbers in_a_row
        let last = getArea(previousNumbers.[0])

        match missedInARow,last with
        | D1,D2 -> Bet.D1_D3
        | D1,D3 -> Bet.D1_D2
        | D2,D1 -> Bet.D2_D3
        | D2,D3 -> Bet.D1_D2
        | D3,D1 -> Bet.D1_D2
        | D3,D2 -> Bet.D1_D3
        | _ -> Bet.Skip