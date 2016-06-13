
:- dynamic dirty/2.
:- dynamic obstacle/3.
:- dynamic in/2.
:- dynamic weight/3.
:- dynamic choose/1.
:- dynamic leftmoves/1.
:- dynamic discover/2.
:- dynamic done/1.
:- dynamic facing/1.
:- dynamic timePassed/1.

do(suck) :- not(done(stop)), in(X,Y), dirty(X,Y).
do(stop) :- not(done(stop)), not(done(start)), in(X,Y), not(dirty(X,Y)), leftmoves(0).
do(wait) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(1), facing(A), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), obstacle(Temp1, Temp2, dynamic).
/*do(forward) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), choose(M), Temp is M+360, facing(Temp).
do(turn90) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), choose(M), Temp
is M+360, not(facing(Temp)).*/
do(forward) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), choose(M), facing(M).
do(turn90) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), choose(M), not(facing(M)).
do(start) :- done(stop), timePassed(T), T = 21600.
