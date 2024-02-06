import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import './Card.css'
export default function Card({ item }) {
    const navigate = useNavigate()
    function getDetails(itemId) {
        navigate('/films/id=' + itemId, { replace: true });
    }
    return (
        <div >
            <div className="image" onClick={() => { getDetails(item.id) }}>
                <div className="genre">{item.genre}</div>

                <img src={item.coverImagePath} alt="img" />

                <img src={item.coverImagePath} alt="img" />

            </div>
            <div className="infoFilm">
                <div className='name'>{item.name}</div>
                <div className='ageLimit'>{item.ageLimit}+</div>
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
