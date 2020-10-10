module study_DU_with_variable


let values = (1,2,3)
let valueToLookFor = 2

// working version
let result_ok = 
    match values with
    | (x,_,_) when x = valueToLookFor -> "pos 1"
    | (_,x,_) when x = valueToLookFor -> "pos 2"
    | (_,_,x) when x = valueToLookFor -> "pos 3"
    | _ -> "not found"


// simpler version

let x1 = valueToLookFor
let x2 = valueToLookFor
let x3 = valueToLookFor

let result_simpler =
    match values with
    | (x1,_,_) -> "pos 1"   // ALWAYS match
    | (_,x2,_) -> "pos 2"
    | (_,_,x3) -> "pos 3"
    | _ -> "not found"


// wanted version

let x = valueToLookFor

let result_wanted =
    match values with
    | (x,_,_) -> "pos 1"    // ALWAYS match
    | (_,x,_) -> "pos 2"
    | (_,_,x) -> "pos 3"
    | _ -> "not found"


