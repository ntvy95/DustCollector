%start:- assert(in(0,0)), assert(weight(0,0,1), assert(choose(0)),
% assert(leftmoves(1)), assert(discover(0,0)).
:- dynamic dirty/2.
:- dynamic obstacle/3.
:- dynamic in/2.
:- dynamic weight/3.
:- dynamic choose/1.
:- dynamic leftmoves/1.
:- dynamic discover/2.
:- dynamic timez/2.
dirty(a,b).
obstacle(a,b,both).
assert(in(0,0)).
assert(weight(0,0,1)).
assert(choose(0)).
assert(leftmoves(1)).
assert(discover(0,0)).
assert(facing(0)).
assert(do(start)).

do(suck):- not(do(stop)), in(X,Y), dirty(X,Y).
do(stop):- not(do(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(0).
do(wait):- not(do(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(1), facing(A), obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), dynamic).
do(forward):- not(do(stop)), in(X, Y), not(dirty(X,Y)), leftmoves(K), K > 0, choose(M), facing(M + 360).
do(turn90):- not(do(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(K), K > 0, choose(M), not(facing(M + 360)).
do(start):- timez(T), T =:= 21600.


facing(A+90):- facing(A), do(turn90).
%r2 { in(X, Y), facing(A), weight(X, Y, W), do(forward) } → { in(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))), weight(X, Y, W + 1), choose(A) }
in(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))):- in(X,Y), facing(A), do(forward).
weight(X,Y,W + 1):- in(X,Y), weight(X,Y,W), do(forward).
choose(A):- facing(A), do(forward).

choose(A):- in(X,Y), facing(A), choose(M), obstacle(X + cos(M), Y + sin(M), both), do(turn90).
choose(A):- facing(A), in(X, Y), not(obstacle(X + cos(M), Y + sin(M),both)), not(obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), both)), choose(M), weight(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), W1), weight(X + cos(M), Y + sin(M), W2), W1 < W2,  do(turn90).
%r5 { facing(a), in(X, Y), not(obstacle(x + round(cos(pi*A/180)), y + round(sin(pi*A/180)), static)), not(discover(x + round(cos(pi*A/180)), y + round(sin(pi*A/180)))), leftmoves(k) } → { discover(x + round(cos(pi*A/180)), y + round(sin(pi*A/180))), leftmoves(k + 1) }

discover(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))):- not(do(stop)), facing(A), in(X,Y), not(obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), static)), not(discover(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)))).
leftmoves(K+1):- not(do(stop)), facing(A), in(X,Y), not(obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), static)), not(discover(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)))), leftmoves(K).

leftmoves(K-1):- in(X,Y), weight(X,Y,1), leftmoves(K), do(forward).
timez(T+1):- timez(T), do(stop).

%r8
in(0,0):- do(start).
weight(0,0,1):- do(start).
choose(0):- do(start).
leftmoves(1):- do(start).
discover(0,0):- do(start).
facing(0):- do(start).

