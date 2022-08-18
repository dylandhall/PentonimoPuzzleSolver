# Pentonimo Puzzle Solver

Solves an interesting 10x10 grid of pentonimos. The puzzle can be found here: https://thinksquare.com.au/store/puzzles/worlds-hardest-12-piece-puzzle/

This is a great little puzzle that was dropped at my office years ago.

No one could solve it, and I couldn't find the instructions for it online, so I started to suspect that we'd simply misunderstood the puzzle. So I wrote an app to solve it - but my first implementation was so slow would have taken years to run! Turns out a naive solution just wouldn't cut it.

I worked out some hacks to refine performance and got it to run in a reasonable time, and proved myself wrong - the puzzle does indeed have a solution!

Here's the source code for the solver. It's really for my eyes only, I've tidied it slightly before sharing it in case anyone wants to read it however. I've also done a few little tweaks and it now runs in about half an hour on my new laptop - originally it took a day an a half.

I'd be very interested in any ideas to further increase the performance, I'm sure there are some smart hacks to get it down to a few minutes. Very happy to have found a fast way to brute force this however.
