import './stars.css';
const Star = (props) => {
    return (
        <svg width={props.width} height={props.height} viewBox="0 0 67 66" xmlns="http://www.w3.org/2000/svg">
            <path d="M21.7635 19.2156L4.94684 24.1375C3.55953 24.5436 3.04788 26.2419 3.98005 27.3467L15.0286 40.4413C15.333 
            40.8021 15.5 41.2589 15.5 41.731V59.8866C15.5 61.3431 17.0069 62.3111 18.3315 61.7056L32.221 55.3561C32.7185 55.1287 
            33.2874 55.115 33.7953 55.3181L50.2572 61.9029C51.5709 62.4284 53 61.4609 53 60.0459V41.6442C53 41.2253 53.1315 40.817
             53.376 40.4769L62.8755 27.2602C63.6736 26.1497 63.1467 24.5836 61.8396 24.1814L45.6522 19.2007C45.2294 19.0706 44.8616 
             18.8039 44.6065 18.4425L34.7452 4.4724C33.9182 3.30082 32.1633 3.35468 31.4097 4.57478L22.9033 18.3471C22.6437 18.7674 
             22.2376 19.0768 21.7635 19.2156Z" fill={props.filled ? "#FFD930" : "none"} stroke="#FFD930" stroke-width="6" />
        </svg>
    )
}

const Stars = (props) => {
    const stars = [];
    for (let i = 0; i < 5; i++) {
        stars.push(<Star width={45} height={46} filled={i < props.rating || i === props.rating} />);
    }

    return (
        <div className="stars">
            {stars}
        </div>
    );
}
export default Stars;

