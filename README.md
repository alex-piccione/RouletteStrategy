# Roulette Strategy
Practice with F# while test some Roulette strategies

I bought this item on ebay:  
> "ROULETTE SYSTEM - Safe Easy Roulette Dozens Strategy | £480 Profit!" 
from maxpell2020@gmail.com

I was just curious to see what this kind of "strategy" can it be.
Essentially it says to bet against the dozen that didn't appeared in the previous 5 spins (bet on the other 2 dozens).

I think that it is counterintuitive, because with a high number of events the distribution will be uniform on the possibilities.  
I wrote a console application that simulate playing that strategy.  

I added a "random" strategy and a strategy that bet against the dozen that didn't appeared in the last N spins plus the other dozen that didn't appeared in the last spin.  
With both these strategies I obtained a Win/Lost ratio better than the suggested strategy!

