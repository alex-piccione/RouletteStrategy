﻿namespace RouletteStrategy

module Entities =

    type Area =
        | Zero
        //| DoubleZero
        | D1 // 1 to 12
        | D2 // 13 to 24
        | D3 // 25 to 36


    type Suggestion =
        | None
        | D1_D2   //  1-12 + 13-24
        | D1_D3   //  1-12 + 25-36
        | D2_D3   // 23-24 + 25-36


    type SpinResult =
        | Won
        | Lost
        | Skip