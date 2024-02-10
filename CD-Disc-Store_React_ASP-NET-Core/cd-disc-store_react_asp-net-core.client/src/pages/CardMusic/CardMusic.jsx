import React, { useEffect, useState } from 'react'
import './CardMusic.css'
export default function CardMusic({ item }) {
    return (
        <div>
            <div className="image">
                <div className='genre'>{item.genre}</div>
                <img src={item.coverImagePath} alt="img" />

            </div>
            <div className="infoMusic">
                <div className='name'>{item.name}</div>
                <div className="descriptoin">{item.artist}</div>
                <div className="language">{item.language}</div>
                <div className='forPurchase'>
                    <div className="btn_cart">
                        <button>&#10027; In Cart</button>
                    </div>
                </div>
            </div>
        </div>
    )
}