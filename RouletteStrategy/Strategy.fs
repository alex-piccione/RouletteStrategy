namespace RouletteStrategy

open Entities

module Strategy =

    let getArea number =
        match number with 
        | n when n>=1 && n<=12 -> Area.One
        | n when n>=13 && n<=24 -> Area.Two
        | n when n>=25 && n<=36 -> Area.Three
        | _ -> Area.Zero

    let strategy_2 = fun previousNumbers in_a_row ->
        // if there is a dozen that didn't appered in the last PREVIOUS_NUMBERS suggest the other 2 dozens (bet against it)
        // it is not perfect because it does not consider a "second place" while choosing the other 2 dozens 
        // but it is a so very rare event both 2 dozens not appear in a row for the same number of slips that can be ignored.

               
        let rec checkPrevious numbers counters =
            // counters contains for how many time the area One, Two and Three have NOT appeared respectively

            let (D1,D2,D3) = counters

            match counters with
            | x,_,_ when x=in_a_row -> Area.One
            | _,x,_ when x=in_a_row -> Area.Two
            | _,_,x when x=in_a_row -> Area.Three
            | _ ->
                match numbers with
                | [] -> Area.Zero // Area.None // not enough data
                | n::tail ->  
                    let newCounters = 
                        match getArea n with
                        | Area.One -> (0, D2+1, D3+1)
                        | Area.Two -> (D1+1, 0, D3+1)
                        | Area.Three -> (D1+1, D2+1, 0)
                        | _ -> (D1+1, D2+1, D3+1)

                    checkPrevious tail newCounters  

        let notAppearedArea = checkPrevious previousNumbers (0,0,0)

        match notAppearedArea with
        | Area.One -> Suggestion.D2_D3  
        | Area.Two -> Suggestion.D1_D3
        | Area.Three -> Suggestion.D1_D2
        | _ -> Suggestion.None



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