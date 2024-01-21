import React, { useEffect, useState } from 'react'
import './Card.css'
export default function Card({ item }) {

    return (
        <div className='card'>
            <div className="image">
                <div className='genre'>{item.genre}</div>
                <img src="https://w0.peakpx.com/wallpaper/627/792/HD-wallpaper-no-black-blank.jpg" alt="img" />
            </div>
            <div className="infoFilm">
                <div className='name'>{item.name}</div>
                <div className="mainRole">{item.mainRole}</div>
                <div className="descriptoin">{item.producer}</div>
                <div className='forPurchase'>
                    <div className="price">100$</div>
                    <div className="btn_cart">
                        <button>&#10027; In Cart</button>
                    </div>
                </div>

            </div>
        </div>
    )
}
