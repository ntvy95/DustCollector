%assert(in(0,0)), assert(weight(0,0,1)), assert(choose(0)), assert(leftmoves(1)), assert(discover(0,0)), assert(facing(0)), do(X).

:- dynamic dirty/2.
:- dynamic obstacle/3.
:- dynamic in/2.
:- dynamic weight/3.
:- dynamic choose/1.
:- dynamic leftmoves/1.
:- dynamic discover/2.
:- dynamic do/1.
:- dynamic facing/1.



dirty(a,b).
obstacle(a,b,both).

do(suck):- nb_getval(done,D), D \= stop, in(X,Y), dirty(X,Y), nb_setval(done,suck).
do(stop):- nb_getval(done,D), D \= stop, in(X,Y), not(dirty(X,Y)), leftmoves(0), nb_setval(done,stop).
do(wait):- nb_getval(done,D), D \= stop, in(X,Y), not(dirty(X,Y)), leftmoves(1), facing(A), obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), dynamic), nb_setval(done,wait).
do(forward):- nb_getval(done,D), D \= stop, in(X, Y), not(dirty(X,Y)), leftmoves(K), K > 0, choose(M), facing(M + 360), nb_setval(done,forward).
do(turn90):- nb_getval(done,D), D \= stop, in(X,Y), not(dirty(X,Y)), leftmoves(K), K > 0, choose(M), not(facing(M + 360)), nb_setval(done,turn90).
do(start):- time(T), T = 21600, nb_setval(done,start).


facing(A+90):- facing(A), nb_getval(done,D), D = turn90.
%r2 { in(X, Y), facing(A), weight(X, Y, W), do(forward) } → { in(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))), weight(X, Y, W + 1), choose(A) }
in(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))):- in(X,Y), facing(A), nb_getval(done,D), D = forward.
weight(X,Y,W + 1):- in(X,Y), weight(X,Y,W), nb_getval(done,D), D = forward.
choose(A):- facing(A), nb_getval(done,D), D = forward.

choose(A):- in(X,Y), facing(A), choose(M), obstacle(X + cos(M), Y + sin(M), both), nb_getval(done,D), D = turn90.
choose(A):- facing(A), in(X, Y), not(obstacle(X + cos(M), Y + sin(M),both)), not(obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), both)), choose(M), weight(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), W1), weight(X + cos(M), Y + sin(M), W2), W1 < W2,  nb_getval(done,D), D = turn90.
%r5 { facing(a), in(X, Y), not(obstacle(x + round(cos(pi*A/180)), y + round(sin(pi*A/180)), static)), not(discover(x + round(cos(pi*A/180)), y + round(sin(pi*A/180)))), leftmoves(k) } → { discover(x + round(cos(pi*A/180)), y + round(sin(pi*A/180))), leftmoves(k + 1) }

discover(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))):- nb_getval(done,D), D \= stop, facing(A), in(X,Y), not(obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), static)), not(discover(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)))).
leftmoves(K+1):- nb_getval(done,D), D \= stop, facing(A), in(X,Y), not(obstacle(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)), static)), not(discover(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180)))), leftmoves(K).

leftmoves(K-1):- in(X,Y), weight(X,Y,1), leftmoves(K), nb_getval(done,D), D = forward.
time(T+1):- time(T), nb_getval(done,D), D = stop.

%r8
in(0,0):- nb_getval(done,D), D = start.
weight(0,0,1):- nb_getval(done,D), D = start.
choose(0):- nb_getval(done,D), D = start.
leftmoves(1):- nb_getval(done,D), D = start.
discover(0,0):- nb_getval(done,D), D = start.
facing(0):- nb_getval(done,D), D = start.

