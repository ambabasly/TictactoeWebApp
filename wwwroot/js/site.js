// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let gameStatus = document.querySelector(".game--status");
let cellStatus = ["","","","","","","","",""];
let firstPlayer = "X";
let secondPlayer = "O";
let player1turn = true;
let gameOver = false;

function onCellClick(cellEvent) {
    const clickedCell = cellEvent.target;
    
    const cellNumber = parseInt(clickedCell.getAttribute("data-cell-index"));

    if (cellStatus[cellNumber] !== "" || gameOver) {
        return;
    }
    
    if (player1turn) {
        cellStatus[cellNumber] = firstPlayer;
        clickedCell.innerHTML = firstPlayer;
        player1turn = false;
    }
    else {
        cellStatus[cellNumber] = secondPlayer;
        clickedCell.innerHTML = secondPlayer;
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

        if (x === "" || y === "" || z === "") {
            continue;
        }

        if (x === y && y === z) {
            
            gameOver = true;
            gameStatus.innerHTML = "Winner";
            break;
        }
    }
}

function newGame() {
    player1turn = true;
    gameOver = false;
    cellStatus = ["", "", "", "", "", "", "", "", ""];
    document.querySelectorAll(".cell").forEach(cell => cell.innerHTML = "");    
    gameStatus.innerHTML = "";
    console.log(cellStatus);
}


document.querySelectorAll(".cell").forEach(cell => cell.addEventListener("click", onCellClick));

$(document).ready(function () {
    $('#contact_form').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            first_name: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Please enter your First Name'
                    }
                }
            },
            last_name: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Please enter your Last Name'
                    }
                }
            },
            user_name: {
                validators: {
                    stringLength: {
                        min: 8,
                    },
                    notEmpty: {
                        message: 'Please enter your Username'
                    }
                }
            },
            user_password: {
                validators: {
                    stringLength: {
                        min: 8,
                    },
                    notEmpty: {
                        message: 'Please enter your Password'
                    }
                }
            },
            confirm_password: {
                validators: {
                    stringLength: {
                        min: 8,
                    },
                    notEmpty: {
                        message: 'Please confirm your Password'
                    }
                }
            },
            email: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your Email Address'
                    },
                    emailAddress: {
                        message: 'Please enter a valid Email Address'
                    }
                }
            },
            contact_no: {
                validators: {
                    stringLength: {
                        min: 12,
                        max: 12,
                        notEmpty: {
                            message: 'Please enter your Contact No.'
                        }
                    }
                },
                department: {
                    validators: {
                        notEmpty: {
                            message: 'Please select your Department/Office'
                        }
                    }
                },
            }
        }
    })
        .on('success.form.bv', function (e) {
            $('#success_message').slideDown({ opacity: "show" }, "slow") // Do something ...
            $('#contact_form').data('bootstrapValidator').resetForm();

            // Prevent form submission
            e.preventDefault();

            // Get the form instance
            var $form = $(e.target);

            // Get the BootstrapValidator instance
            var bv = $form.data('bootstrapValidator');

            // Use Ajax to submit form data
            $.post($form.attr('action'), $form.serialize(), function (result) {
                console.log(result);
            }, 'json');
        });
});