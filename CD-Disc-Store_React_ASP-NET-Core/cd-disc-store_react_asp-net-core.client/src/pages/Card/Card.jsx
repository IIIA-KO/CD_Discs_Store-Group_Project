import React, { useEffect, useState } from 'react'
import './Card.css'
export default function Card({item}) {

    return (
        <div>
            <div className="image">
                <img src="https://w0.peakpx.com/wallpaper/627/792/HD-wallpaper-no-black-blank.jpg" alt="img" />
            </div>
            <div className="info">
                <div className='name'>{item.name}</div>
                <div className="descriptoin">{item.producer}</div>
                <div >
                    <div className="price">100$</div>
                    <div className="btn_cart">
                    <button>In Cart</button>
                    </div>
                </div>

            </div>
        </div>
    )
}
