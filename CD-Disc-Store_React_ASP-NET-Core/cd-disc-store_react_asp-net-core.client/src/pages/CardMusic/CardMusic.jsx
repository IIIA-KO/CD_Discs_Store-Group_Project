import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import './CardMusic.css'
export default function CardMusic({ item }) {
    const navigate = useNavigate()
    function getDetails(itemId) {
        navigate('/music/id=' + itemId, { replace: true });
    }
    return (
        <div>
            <div className="image" onClick={() => { getDetails(item.id) }}>
                <div className="genre">{item.genre}</div>
                <img src={ item.coverImagePath } alt="img" />
            </div>
            <div className="infoMusic">
                <div className='name'>{item.name}</div>
                <div className="descriptoin">{item.artist}</div>
                <div className="language">{item.language}</div>
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
