namespace RouletteStrategy

open Entities

module Strategy =

    let getArea number =
        match number with 
        | n when n>= 1 && n<=12 -> Area.D1
        | n when n>=13 && n<=24 -> Area.D2
        | n when n>=25 && n<=36 -> Area.D3
        | _ -> Area.Zero

    let strategy_2 = fun (previousNumbers:int list) in_a_row ->
        // if there is a dozen that didn't appered in the last PREVIOUS_NUMBERS suggest the other 2 dozens (bet against it)
        // it is not perfect because it does not consider a "second place" while choosing the other 2 dozens 
        // but it is a so very rare event both 2 dozens not appear in a row for the same number of slips that can be ignored.

               
        let rec checkPrevious numbers counters =
            // counters contains for how many time the area One, Two and Three have NOT appeared respectively

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

        let numbersToCheck = previousNumbers.[..in_a_row]
        let notAppearedArea = checkPrevious numbersToCheck (0,0,0)

        match notAppearedArea with
        | Area.D1 -> Bet.D2_D3  
        | Area.D2 -> Bet.D1_D3
        | Area.D3 -> Bet.D1_D2
        | _ -> Bet.Skip



    //let strategy_1 = fun previousNumbers ->
    //    // if the last PREVIOUS_NUMBERS numbers are on the same Area we suggest the other 2 areas 

    //    let rec checkPrevious numbers (currentArea:Area) count =
    //        if count = PREVIOUS_NUMBERS then currentArea
    //        else
    //        match numbers with
    //        | [] -> Area.None // area, count
    //        | n::tail ->
    //            let area = getArea n
    //            if count = 0 then checkPrevious tail area 1
    //            else 
    //                if area = currentArea then checkPrevious tail area (count+1)                    
    //                else Area.None

    //    let area = checkPrevious previousNumbers Area.None 0 

    //    match area with
    //    | Area.One -> Suggestion.Two_Three
    //    | Area.Two -> Suggestion.Two_Three
    //    | Area.Three -> Suggestion.One_Two
    //    | _ -> Suggestion.None