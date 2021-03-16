// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let cellStatus = new Array(9);
let firstPlayer = "X";
let secondPlayer = "O";
let player1turn = true;
let gameOver = false;

function onCellClick(cellEvent) {
    const clickedcell = cellEvent.target;
    
    const cellNumber = parseInt(clickedcell.getAttribute("data-cell-index"));

    if (cellStatus[cellNumber] !== undefined || gameOver) {
        return;
    }
    

    if (player1turn) {
        cellStatus[cellNumber] = firstPlayer;
        clickedcell.innerHTML = firstPlayer;
        player1turn = false;
    }
    else {
        cellStatus[cellNumber] = secondPlayer;
        clickedcell.innerHTML = secondPlayer;
        player1turn = true;
    }        

    checkForWinner();
}



function checkForWinner() {
    let winningConditions = [
        [0, 1, 2], [3, 4, 5],
        [6, 7, 8], [0, 3, 6],
        [1, 4, 7], [2, 5, 8],
        [0, 4, 8], [2, 4, 6],
    ];

    for (let i = 0; i <= 7; i++) {
        const win = winningConditions[i];
        let x = cellStatus[win[0]];
        let y = cellStatus[win[1]];
        let z = cellStatus[win[2]];

        if (x === undefined || y === undefined || z === undefined) {
            continue;
        }

        if (x === y && y === z) {
            
            console.log('there\'s a win!');
            gameOver = true;
            newGame();
            break;
        }
    }
}

function newGame() {
    document.querySelectorAll(".cell").forEach(cell => cell.innerHTML = "");
    cellStatus = new Array(9);
}



document.querySelectorAll(".cell").forEach(cell => cell.addEventListener("click", onCellClick));